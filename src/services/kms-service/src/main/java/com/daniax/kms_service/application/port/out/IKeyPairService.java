package com.daniax.kms_service.application.port.out;

import com.daniax.kms_service.application.utils.AlgorithmType;

import java.security.NoSuchAlgorithmException;

public interface IKeyPairService {
    String[] generateKeys(AlgorithmType algorithmType)  throws NoSuchAlgorithmException; // Generate PK and SK
}
