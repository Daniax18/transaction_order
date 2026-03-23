package com.daniax.kms_service.infrastructure.adapter.out.persistence;

import com.daniax.kms_service.application.port.out.IUserKeyRepository;
import com.daniax.kms_service.application.utils.UserKeysResponse;
import com.daniax.kms_service.domain.entity.KeyStatus;
import com.daniax.kms_service.domain.entity.UserKeys;
import com.daniax.kms_service.domain.exception.ResourceNotFoundException;
import com.daniax.kms_service.infrastructure.repository.UserKeyJpaRepository;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class UserKeyRepositoryAdapter implements IUserKeyRepository {

    private final UserKeyJpaRepository userKeyJpaRepository;

    public UserKeyRepositoryAdapter(UserKeyJpaRepository userKeyJpaRepository) {
        this.userKeyJpaRepository = userKeyJpaRepository;
    }

    @Override
    public UserKeys save(UserKeys keys) {
        UserKeysJpaEntity result = userKeyJpaRepository.save(toJpaEntity(keys));
        return toEntity(result);
    }

    @Override
    public List<UserKeysResponse> findByUserId(String userId) {
        return userKeyJpaRepository.findByUserId(userId);
    }

    @Override
    public List<UserKeysResponse> findValidKeyByUserId(String userId, KeyStatus status) {
        return userKeyJpaRepository.findValidKeysByUserId(userId, status);
    }

    @Override
    public UserKeys findKeyById(Long id) {
        Optional<UserKeysJpaEntity> temp = userKeyJpaRepository.findById(id);
        if(temp.isEmpty()) throw new ResourceNotFoundException("Key with id " + id + " not found");
        return toEntity(temp.get());
    }

    @Override
    public boolean existsByUserIdAndKeyName(String userId, String keyName) {
        return false;
    }

    private UserKeysJpaEntity toJpaEntity(UserKeys key) {
        UserKeysJpaEntity e = new UserKeysJpaEntity();
        e.setId(key.getId());
        e.setUserId(key.getUserId());
        e.setKeyName(key.getKeyName());
        e.setPublicKey(key.getPublicKey());
        e.setStatus(key.getStatus());
        e.setCreatedAt(key.getCreatedAt());
        e.setExpiredAt(key.getExpiredAt());
        return e;
    }

    private UserKeys toEntity(UserKeysJpaEntity jpaKeys){
        UserKeys userKeys = new UserKeys();
        userKeys.setId(jpaKeys.getId());
        userKeys.setUserId(jpaKeys.getUserId());
        userKeys.setKeyName(jpaKeys.getKeyName());
        userKeys.setPublicKey(jpaKeys.getPublicKey());
        userKeys.setStatus(jpaKeys.getStatus());
        userKeys.setCreatedAt(jpaKeys.getCreatedAt());
        userKeys.setExpiredAt(jpaKeys.getExpiredAt());

        return  userKeys;

    }
}
