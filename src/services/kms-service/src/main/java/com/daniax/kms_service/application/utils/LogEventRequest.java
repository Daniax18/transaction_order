package com.daniax.kms_service.application.utils;

import java.time.Instant;

public record LogEventRequest(
        String UserId,
        String ServiceName,
        String ActionName,
        String ActionStatus,
        Instant ActionTime
) {
}
