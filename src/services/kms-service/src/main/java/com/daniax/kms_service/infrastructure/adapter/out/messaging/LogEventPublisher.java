package com.daniax.kms_service.infrastructure.adapter.out.messaging;

import com.daniax.kms_service.application.port.out.ILogService;
import com.daniax.kms_service.application.utils.LogEventRequest;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.stereotype.Service;

@Service
public class LogEventPublisher implements ILogService {

    private final RabbitTemplate rabbitTemplate;

    public LogEventPublisher(RabbitTemplate rabbitTemplate) {
        this.rabbitTemplate = rabbitTemplate;
    }

    @Override
    public void logInfo(LogEventRequest log) {
        rabbitTemplate.convertAndSend(
                "",
                "log.created",
                log
        );
    }
}
