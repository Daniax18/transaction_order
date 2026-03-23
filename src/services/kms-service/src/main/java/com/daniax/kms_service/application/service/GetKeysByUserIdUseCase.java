package com.daniax.kms_service.application.service;

import com.daniax.kms_service.application.port.in.IGetKeysByUserIdUseCase;
import com.daniax.kms_service.application.port.out.IUserKeyRepository;
import com.daniax.kms_service.application.utils.UserKeysResponse;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class GetKeysByUserIdUseCase implements IGetKeysByUserIdUseCase {

    private final IUserKeyRepository userKeyRepository;

    public GetKeysByUserIdUseCase(IUserKeyRepository userKeyRepository) {
        this.userKeyRepository = userKeyRepository;
    }

    @Override
    public List<UserKeysResponse> getAllKeysByUserId(String userId) {
        return userKeyRepository.findByUserId(userId);
    }
}
