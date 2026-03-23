package com.daniax.kms_service.application.utils;

public enum AlgorithmType {
    RSA("RSA"),
    SHA256_WITH_RSA("SHA256withRSA");

    private final String value;

    AlgorithmType(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
