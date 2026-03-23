package com.daniax.kms_service.application.port.in;

import com.daniax.kms_service.application.utils.UserKeysResponse;

import java.util.List;

public interface IGetKeysByUserIdUseCase {
    List<UserKeysResponse> getAllKeysByUserId(String userId);
}
