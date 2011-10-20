package pt.isel.deetc.si.deCipher;

import pt.isel.deetc.common.clp.CommandLineParser;

import javax.crypto.Cipher;
import javax.crypto.CipherInputStream;
import javax.crypto.NoSuchPaddingException;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.*;
import java.util.Map;

public class Decipher {

    private static final String _keyStoreType = "PKCS12";
    public static void main(String[] args) {
        Map<String, String> params;
        CommandLineParser parser = new CommandLineParser();
        params = parser.parse(args);

        try {
            File metaFile = new File(params.get("metafile"));
            File cipherFile = new File(params.get("cipherfile"));
            File saveFile = new File(metaFile.getAbsolutePath().substring(metaFile.getAbsolutePath().length() - 3));

            FileInputStream metaStream = new FileInputStream(metaFile);
            FileInputStream cipherStream = new FileInputStream(cipherFile);
            FileInputStream keyStoreStream = new FileInputStream(params.get("keystore"));
            FileOutputStream saveStream = new FileOutputStream(saveFile);

            KeyStore keyStore = KeyStore.getInstance(_keyStoreType);
            keyStore.load(keyStoreStream, params.get("password").toCharArray());

            Metadata meta = Metadata.ReadFromFile(metaStream);
            PrivateKey key = (PrivateKey) keyStore.getKey(keyStore.getCertificateAlias(meta.getCertificate()), params.get("password").toCharArray());
            DecipherFile(cipherStream, saveStream, key, meta);

            System.out.println("C'est fini!");
        } catch (Exception e) {
            System.err.println(e.getMessage());
        }
    }

    private static void DecipherFile(FileInputStream cipherStream, FileOutputStream fileStream, PrivateKey key, Metadata meta) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IOException {
        Cipher cipher = Cipher.getInstance(meta.getAssimetricAlgorithm());
        cipher.init(Cipher.UNWRAP_MODE, key);

        Key cipherKey = cipher.unwrap(meta.getWrappedKey(), meta.getSimetricAlgorithm(), Cipher.SECRET_KEY);

        cipher = Cipher.getInstance(meta.getSimetricAlgorithm());
        cipher.init(Cipher.DECRYPT_MODE, cipherKey);

        CipherInputStream inputStream = new CipherInputStream(cipherStream, cipher);
        byte[] b = new byte[4 * 1024];
        int rd;
        while((rd = inputStream.read(b)) != -1) {
            fileStream.write(b, 0, rd);
        }
    }

}
