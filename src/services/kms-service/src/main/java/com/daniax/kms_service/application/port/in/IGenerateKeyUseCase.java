package com.daniax.kms_service.application.port.in;

import com.daniax.kms_service.application.utils.GeneratedKeyResult;
import com.daniax.kms_service.application.utils.GeneratedKeyRequest;
import com.daniax.kms_service.domain.exception.ResourceAlreadyExistsException;

import java.security.NoSuchAlgorithmException;

public interface IGenerateKeyUseCase {
    GeneratedKeyResult generateKeys(GeneratedKeyRequest generatedKeyRequest) throws ResourceAlreadyExistsException, NoSuchAlgorithmException;
}
