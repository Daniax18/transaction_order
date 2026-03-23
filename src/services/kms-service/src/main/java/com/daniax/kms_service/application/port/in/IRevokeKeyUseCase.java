package com.daniax.kms_service.application.port.in;

import com.daniax.kms_service.application.utils.RevokedKeyResult;

public interface IRevokeKeyUseCase {
    RevokedKeyResult revokeKey(Long id);
}
