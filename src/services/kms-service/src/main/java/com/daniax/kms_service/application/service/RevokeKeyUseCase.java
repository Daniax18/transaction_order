package com.daniax.kms_service.application.service;

import com.daniax.kms_service.application.port.in.IRevokeKeyUseCase;
import com.daniax.kms_service.application.port.out.ILogService;
import com.daniax.kms_service.application.port.out.IUserKeyRepository;
import com.daniax.kms_service.application.utils.AuditType;
import com.daniax.kms_service.application.utils.LogEventRequest;
import com.daniax.kms_service.application.utils.RevokedKeyResult;
import com.daniax.kms_service.domain.entity.KeyStatus;
import com.daniax.kms_service.domain.entity.UserKeys;
import org.springframework.stereotype.Service;

import java.time.Instant;

@Service
public class RevokeKeyUseCase implements IRevokeKeyUseCase {

    private final IUserKeyRepository userKeyRepository;
    private final ILogService logService;

    public RevokeKeyUseCase(IUserKeyRepository userKeyRepository, ILogService logService) {
        this.userKeyRepository = userKeyRepository;
        this.logService = logService;
    }

    @Override
    public RevokedKeyResult revokeKey(Long id) {
        UserKeys uk = userKeyRepository.findKeyById(id);
        uk.setStatus(KeyStatus.REVOKED);
        uk = userKeyRepository.save(uk);

        logService.logInfo(new LogEventRequest(
                uk.getUserId(),
                "KMS-SERVICE",
                AuditType.REVOKED.getValue(),
                "SUCCESS",
                Instant.now()
        ));

        return new RevokedKeyResult(uk.getKeyName());
    }
}
