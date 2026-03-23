package com.daniax.kms_service.application.port.in;

import com.daniax.kms_service.application.utils.UserKeysResponse;

import java.util.List;

public interface IGetKeysValidByUserIdUseCase {
    List<UserKeysResponse> getKeysValidByUserId(String userId);
}
