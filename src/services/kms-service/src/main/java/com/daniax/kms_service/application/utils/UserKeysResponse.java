package com.daniax.kms_service.application.utils;

import com.daniax.kms_service.domain.entity.KeyStatus;

public record UserKeysResponse(
        Long id,
        String userId,
        String keyName,
        KeyStatus status,
        String publicKey
) {
}
