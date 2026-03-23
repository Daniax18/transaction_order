package com.daniax.kms_service.application.utils;

public enum AuditType {
    CREATION("CREATION"),
    REVOKED("REVOKED");

    private final String value;

    AuditType(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
