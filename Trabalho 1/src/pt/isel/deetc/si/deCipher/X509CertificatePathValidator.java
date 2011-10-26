package pt.isel.deetc.si.deCipher;

import java.security.InvalidAlgorithmParameterException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.cert.*;
import java.util.List;

public class X509CertificatePathValidator {

    private final List<Certificate> _certificates;
    private final KeyStore _trustAnchors;
    private final String _certStoreType;
    private final String _certPathType;

    public X509CertificatePathValidator(List<Certificate> certificates, KeyStore trustAnchors) {
        _certificates = certificates;
        _trustAnchors = trustAnchors;
        _certStoreType = "Collection";
        _certPathType = "PKIX";
    }

    public boolean Validate(Certificate leaf) throws CertificateException, KeyStoreException, InvalidAlgorithmParameterException {
        try{
            CollectionCertStoreParameters certStoreParameters = new CollectionCertStoreParameters(_certificates);

            CertStore store = CertStore.getInstance(_certStoreType, certStoreParameters);
            X509CertSelector certSelector = new X509CertSelector();
            certSelector.setCertificate((X509Certificate) leaf);

            PKIXBuilderParameters pkixBuilderParameters = new PKIXBuilderParameters(_trustAnchors, certSelector);
            pkixBuilderParameters.setRevocationEnabled(false);
            pkixBuilderParameters.addCertStore(store);

            CertPathBuilder certPathBuilder = CertPathBuilder.getInstance(_certPathType);
            PKIXCertPathBuilderResult builderResult = (PKIXCertPathBuilderResult) certPathBuilder.build(pkixBuilderParameters);
            CertPathValidator v = CertPathValidator.getInstance(_certPathType);

            v.validate(builderResult.getCertPath(), pkixBuilderParameters);

            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }

}
