package com.daniax.kms_service.application.utils;

public record GeneratedKeyRequest(
        String userId,
        String keyName,
        int validity
) {
}
