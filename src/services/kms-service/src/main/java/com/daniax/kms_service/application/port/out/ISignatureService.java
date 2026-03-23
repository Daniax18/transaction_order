package com.daniax.kms_service.application.port.out;

import com.daniax.kms_service.application.utils.AlgorithmType;

public interface ISignatureService {
    String signHash(
            String hashBase64,
            String skBase64,
            AlgorithmType keyFactoryInstance,
            AlgorithmType signatureInstance
    );

    boolean isSignatureOk(
            String hashBase64,
            String signatureBase64,
            String pkBase64,
            AlgorithmType keyFactoryInstance,
            AlgorithmType signatureInstance
    );
}
