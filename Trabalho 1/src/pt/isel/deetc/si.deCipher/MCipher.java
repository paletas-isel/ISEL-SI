package pt.isel.deetc.si.deCipher;

import pt.isel.deetc.common.clp.CommandLineParser;
import sun.security.provider.certpath.PKIXCertPathValidator;

import javax.crypto.*;
import java.io.*;
import java.security.*;
import java.security.cert.*;
import java.security.cert.Certificate;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

public class MCipher {
    private static final String certType = "x509";

    public static void main(String[] args) {
        Map<String, String> params;
        CommandLineParser parser = new CommandLineParser();
        params = parser.parse(args);

        try {
            final String simetricAlg = "AES";
            final String asimetricAlg = "RSA";

            File infile = new File(params.get("file"));
            File encfile = new File(String.format("%s.enc", infile.getAbsolutePath(), infile.getName()));
            File metafile = new File(String.format("%s.meta", infile.getAbsolutePath(), infile.getName()));
            File certsFolder = new File(params.get("certsFolder"));

            FileInputStream stream = new FileInputStream(infile);
            FileInputStream certStream = new FileInputStream(params.get("cert"));
            FileInputStream keyStoreStream = new FileInputStream(params.get("keystore"));
            FileOutputStream encstream = new FileOutputStream(encfile);
            FileOutputStream metastream = new FileOutputStream(metafile);

            CertificateFactory certificateFactory = CertificateFactory.getInstance(certType);
            Certificate receiverCert = certificateFactory.generateCertificate(certStream);

            KeyStore keyStore = KeyStore.getInstance("JKS");
            keyStore.load(keyStoreStream, params.get("password").toCharArray());

            if(ValidateCert(receiverCert, certsFolder, keyStore)) {
                KeyGenerator secKeyGen = KeyGenerator.getInstance(simetricAlg);
                Key key = secKeyGen.generateKey();

                PleaseDoCipher(simetricAlg, stream, encstream, key);
                PleaseDoMeta(asimetricAlg, metastream, receiverCert, key);
            }
            else
                System.err.println("Validation failed.");
        } catch (Exception e) {
            System.err.println(e.getMessage());
        }
    }

    private static boolean ValidateCert(Certificate receiverCert, File certsFolder, KeyStore keyStore) throws NoSuchAlgorithmException, KeyStoreException, InvalidAlgorithmParameterException, CertPathBuilderException, CertificateException {
        List<Certificate> certs = new LinkedList<>();
        CertificateFactory certificateFactory = CertificateFactory.getInstance(certType);

        for(File f : certsFolder.listFiles()) {
            try {
                certs.add(certificateFactory.generateCertificate(new FileInputStream(f)));
            } catch (FileNotFoundException | CertificateException e) {
                System.err.println(e.getMessage());
            }
        }

        certs.add(receiverCert);

        CertificateFactory factory = CertificateFactory.getInstance(certType);
        CertPath path = factory.generateCertPath(certs);

        PKIXCertPathValidator validator = new PKIXCertPathValidator();

        PKIXCertPathValidatorResult resultValidator = null;
        try {
            PKIXParameters parameters = new PKIXParameters(keyStore);
            parameters.setRevocationEnabled(false);
            resultValidator = (PKIXCertPathValidatorResult) validator.engineValidate(path, parameters);

            return true;
        } catch (CertPathValidatorException e) {
            return false;
        }
    }

    private static void PleaseDoMeta(String algorithm, FileOutputStream metastream, Certificate receiverCert, Key cipherKey) throws CertificateEncodingException, IOException, NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {
        Cipher cipher = Cipher.getInstance(algorithm);
        cipher.init(Cipher.WRAP_MODE, receiverCert.getPublicKey());

        metastream.write(cipher.wrap(cipherKey));

        metastream.write(receiverCert.getEncoded());
    }

    private static void PleaseDoCipher(String algorithm, FileInputStream stream, FileOutputStream encstream, Key cipherKey) throws NoSuchAlgorithmException, InvalidKeyException, IOException, SignatureException, NoSuchPaddingException, IllegalBlockSizeException, BadPaddingException {
        Cipher cipher = Cipher.getInstance(algorithm);

        cipher.init(Cipher.ENCRYPT_MODE, cipherKey);
        byte[] read = new byte[4 * 1024];
        int nrByteRead;

        while((nrByteRead = stream.read(read)) != -1) {
            encstream.write(cipher.update(read, 0, nrByteRead));
        }

        byte[] signed = cipher.doFinal();
        encstream.write(signed);
    }

}
