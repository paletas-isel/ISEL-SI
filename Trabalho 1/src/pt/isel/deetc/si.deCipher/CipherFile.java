package pt.isel.deetc.si.deCipher;

import pt.isel.deetc.common.clp.CommandLineParser;

import javax.crypto.*;
import java.io.*;
import java.security.*;
import java.security.cert.*;
import java.security.cert.Certificate;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

public class CipherFile {
    private static final String _certType = "x509";
    private static final String _simetricAlg = "AES";
    private static final String _asimetricAlg = "RSA";
    private static final String _keyStoreType = "JKS";

    public static void main(String[] args) {
        long init = System.currentTimeMillis();

        Map<String, String> params;
        CommandLineParser parser = new CommandLineParser();
        params = parser.parse(args);

        try {
            File inFile = new File(params.get("file"));
            File encFile = new File(String.format("%s.enc", inFile.getAbsolutePath(), inFile.getName()));
            File metaFile = new File(String.format("%s.meta", inFile.getAbsolutePath(), inFile.getName()));
            File certsFolder = new File(params.get("certsFolder"));

            FileInputStream stream = new FileInputStream(inFile);
            FileInputStream certStream = new FileInputStream(params.get("cert"));
            FileInputStream keyStoreStream = new FileInputStream(params.get("keystore"));
            FileOutputStream encStream = new FileOutputStream(encFile);
            FileOutputStream metaStream = new FileOutputStream(metaFile);

            CertificateFactory certificateFactory = CertificateFactory.getInstance(_certType);
            Certificate receiverCert = certificateFactory.generateCertificate(certStream);

            KeyStore keyStore = KeyStore.getInstance(_keyStoreType);
            keyStore.load(keyStoreStream, params.get("password").toCharArray());

            if(ValidateCert(receiverCert, certsFolder, keyStore)) {
                KeyGenerator secKeyGen = KeyGenerator.getInstance(_simetricAlg);
                Key key = secKeyGen.generateKey();

                CreateCipherFile(_simetricAlg, stream, encStream, key);
                CreateMetaFile(_simetricAlg, _asimetricAlg, metaStream, receiverCert, key);
            }
            else
                System.err.println("Validation failed.");
        } catch (Exception e) {
            System.err.println(e.getMessage());
        }

        System.out.println((System.currentTimeMillis() - init));
    }

    private static boolean ValidateCert(Certificate receiverCert, File certsFolder, KeyStore keyStore) throws NoSuchAlgorithmException, KeyStoreException, InvalidAlgorithmParameterException, CertPathBuilderException, CertificateException {
        List<Certificate> certs = new LinkedList<>();
        CertificateFactory certificateFactory = CertificateFactory.getInstance(_certType);

        for(File f : certsFolder.listFiles()) {
            try {
                certs.add(certificateFactory.generateCertificate(new FileInputStream(f)));
            } catch (FileNotFoundException | CertificateException e) {
                System.err.println(e.getMessage());
            }
        }

        X509CertificatePathValidator validator = new X509CertificatePathValidator(certs, keyStore);
        return validator.Validate(receiverCert);
    }

    private static void CreateMetaFile(String sAlgorithm, String asAlgorithm, FileOutputStream metastream, Certificate receiverCert, Key cipherKey) throws CertificateEncodingException, IOException, NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException {
        Metadata meta = new Metadata(sAlgorithm, asAlgorithm, cipherKey, receiverCert);

        meta.WriteToFile(metastream);
    }

    private static void CreateCipherFile(String algorithm, FileInputStream stream, FileOutputStream encstream, Key cipherKey) throws NoSuchAlgorithmException, InvalidKeyException, IOException, SignatureException, NoSuchPaddingException, IllegalBlockSizeException, BadPaddingException {
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
