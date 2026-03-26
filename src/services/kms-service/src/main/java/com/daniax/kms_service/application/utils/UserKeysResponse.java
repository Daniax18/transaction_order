package com.daniax.kms_service.application.utils;

import com.daniax.kms_service.domain.entity.KeyStatus;

import java.time.LocalDateTime;

public record UserKeysResponse(
        Long id,
        String userId,
        String keyName,
        KeyStatus status,
        String publicKey,
        LocalDateTime expiredAt
) {
}
