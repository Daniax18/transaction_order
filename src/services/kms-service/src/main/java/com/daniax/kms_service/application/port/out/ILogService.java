package com.daniax.kms_service.application.port.out;

import com.daniax.kms_service.application.utils.LogEventRequest;

public interface ILogService {
    void logInfo(LogEventRequest log);
}
