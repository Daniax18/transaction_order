package com.daniax.kms_service.application.service;

import com.daniax.kms_service.application.port.in.IGenerateKeyUseCase;
import com.daniax.kms_service.application.port.out.IKeyPairService;
import com.daniax.kms_service.application.port.out.ILogService;
import com.daniax.kms_service.application.port.out.ISignatureService;
import com.daniax.kms_service.application.port.out.IUserKeyRepository;
import com.daniax.kms_service.application.utils.*;
import com.daniax.kms_service.domain.entity.KeyStatus;
import com.daniax.kms_service.domain.entity.UserKeys;
import com.daniax.kms_service.domain.exception.ResourceAlreadyExistsException;
import org.springframework.stereotype.Service;

import java.security.NoSuchAlgorithmException;
import java.time.Instant;
import java.time.LocalDateTime;

@Service
public class GenerateKeyUseCase implements IGenerateKeyUseCase {

    private final IKeyPairService keyPairService;
    private final IUserKeyRepository userKeyRepository;
    private final ILogService logService;

    public GenerateKeyUseCase(
            IKeyPairService keyPairService,
            IUserKeyRepository userKeyRepository,
            ILogService logService
    ) {
        this.keyPairService = keyPairService;
        this.userKeyRepository = userKeyRepository;
        this.logService = logService;
    }

    @Override
    public GeneratedKeyResult generateKeys(GeneratedKeyRequest generatedKeyRequest)
            throws ResourceAlreadyExistsException, NoSuchAlgorithmException {
        if(userKeyRepository.existsByUserIdAndKeyName(generatedKeyRequest.userId(), generatedKeyRequest.keyName())){
            throw new ResourceAlreadyExistsException("Le nom de clé " + generatedKeyRequest.keyName() + " already exists");
        }

        String[] keyPair = keyPairService.generateKeys(AlgorithmType.RSA);
        UserKeys entity = new UserKeys();

        entity.setUserId(generatedKeyRequest.userId());
        entity.setKeyName(generatedKeyRequest.keyName());
        entity.setPublicKey(keyPair[0]);
        entity.setStatus(KeyStatus.ACTIVE);
        LocalDateTime now = LocalDateTime.now();
        entity.setCreatedAt(now);
        entity.setExpiredAt(now.plusMonths(generatedKeyRequest.validity()));

        entity = userKeyRepository.save(entity);

        logService.logInfo(new LogEventRequest(
                entity.getUserId(),
                "KMS-SERVICE",
                AuditType.CREATION.getValue(),
                "SUCCESS",
                Instant.now()
        ));

        return new GeneratedKeyResult(entity.getUserId(), entity.getPublicKey(), keyPair[1]);
    }
}
