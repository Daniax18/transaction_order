package com.daniax.kms_service.application.port.out;

import com.daniax.kms_service.application.utils.UserKeysResponse;
import com.daniax.kms_service.domain.entity.KeyStatus;
import com.daniax.kms_service.domain.entity.UserKeys;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

public interface IUserKeyRepository {
    UserKeys save(UserKeys keys);

    List<UserKeysResponse> findByUserId(String userId);

    List<UserKeysResponse> findValidKeyByUserId(String userId, KeyStatus status);

    UserKeys findKeyById(Long id);

    boolean existsByUserIdAndKeyName(String userId, String keyName);
}
