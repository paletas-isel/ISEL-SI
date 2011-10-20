package pt.isel.deetc.si.deCipher;

import pt.isel.deetc.si.exceptions.ShouldNotHappenException;

import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.NoSuchAlgorithmException;
import java.security.cert.Certificate;
import java.security.cert.CertificateEncodingException;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;

public class Metadata {
    private String _simetricAlgorithm;
    private String _assimetricAlgorithm;
    private Key _cypherKey;
    private Certificate _certificate;
    private byte[] _wrappedKey;

    public Metadata(String sAlgorithm, String asAlgorithm, Key cypherKey, Certificate certificate) {
        _simetricAlgorithm = sAlgorithm;
        _assimetricAlgorithm = asAlgorithm;
        _cypherKey = cypherKey;
        _certificate = certificate;
    }

    private Metadata() {

    }

    public static Metadata ReadFromFile(FileInputStream stream) throws IOException, NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException {
        Metadata meta = new Metadata();

        int algorithmSize = stream.read();
        byte[] algorithmByteArray = new byte[algorithmSize];
        stream.read(algorithmByteArray);
        meta._simetricAlgorithm = new String(algorithmByteArray);

        algorithmSize = stream.read();
        algorithmByteArray = new byte[algorithmSize];
        stream.read(algorithmByteArray);
        meta._assimetricAlgorithm = new String(algorithmByteArray);

        int keySize = stream.read();
        byte[] keyByteArray = new byte[keySize];
        stream.read(keyByteArray);
        meta._wrappedKey = keyByteArray;

        try {
            meta._certificate = CertificateFactory.getInstance("X509").generateCertificate(stream);
        } catch (CertificateException e) {
            throw new ShouldNotHappenException();
        }

        return meta;
    }

    public void WriteToFile(FileOutputStream stream) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IOException, IllegalBlockSizeException, CertificateEncodingException {
        Cipher cipher = Cipher.getInstance(_assimetricAlgorithm);
        cipher.init(Cipher.WRAP_MODE, _certificate.getPublicKey());

        stream.write(_simetricAlgorithm.length());
        stream.write(_simetricAlgorithm.getBytes());

        stream.write(_assimetricAlgorithm.length());
        stream.write(_assimetricAlgorithm.getBytes());

        byte[] keyWrap = cipher.wrap(_cypherKey);
        stream.write(keyWrap.length);
        stream.write(keyWrap);

        byte[] certEncode = _certificate.getEncoded();
        stream.write(certEncode);
    }

    public byte[] getWrappedKey() {
        return _wrappedKey;
    }

    public Certificate getCertificate() {
        return _certificate;
    }

    public String getSimetricAlgorithm() {
        return _simetricAlgorithm;
    }

    public String getAssimetricAlgorithm() {
        return _assimetricAlgorithm;
    }
}
