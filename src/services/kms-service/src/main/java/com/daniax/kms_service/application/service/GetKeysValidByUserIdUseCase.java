package com.daniax.kms_service.application.service;

import com.daniax.kms_service.application.port.in.IGetKeysValidByUserIdUseCase;
import com.daniax.kms_service.application.port.out.IUserKeyRepository;
import com.daniax.kms_service.application.utils.UserKeysResponse;
import com.daniax.kms_service.domain.entity.KeyStatus;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class GetKeysValidByUserIdUseCase implements IGetKeysValidByUserIdUseCase {
    private final IUserKeyRepository userKeyRepository;

    public GetKeysValidByUserIdUseCase(IUserKeyRepository userKeyRepository) {
        this.userKeyRepository = userKeyRepository;
    }

    @Override
    public List<UserKeysResponse> getKeysValidByUserId(String userId) {
        return userKeyRepository.findValidKeyByUserId(userId, KeyStatus.ACTIVE);
    }
}
