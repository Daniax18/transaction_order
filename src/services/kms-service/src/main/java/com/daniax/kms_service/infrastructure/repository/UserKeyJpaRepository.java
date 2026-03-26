package com.daniax.kms_service.infrastructure.repository;

import com.daniax.kms_service.application.utils.UserKeysResponse;
import com.daniax.kms_service.domain.entity.KeyStatus;
import com.daniax.kms_service.infrastructure.adapter.out.persistence.UserKeysJpaEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.List;
import java.util.Optional;

public interface UserKeyJpaRepository extends JpaRepository<UserKeysJpaEntity, Long> {

    @Query("""
        SELECT new com.daniax.kms_service.application.utils.UserKeysResponse(
            uk.id,
            uk.userId,
            uk.keyName,
            uk.status,
            uk.publicKey,
            uk.expiredAt
        )
        FROM UserKeysJpaEntity uk
        WHERE uk.userId = :userId
    """)
    List<UserKeysResponse> findByUserId(@Param("userId") String userId);

    Optional<UserKeysJpaEntity> findById(Long id);

    boolean existsByUserIdAndKeyName(String userId, String keyName);

    @Query("""
        SELECT new com.daniax.kms_service.application.utils.UserKeysResponse(
            uk.id,
            uk.userId,
            uk.keyName,
            uk.status,
            uk.publicKey,
            uk.expiredAt
        )
        FROM UserKeysJpaEntity uk
        WHERE uk.userId = :userId
          AND (uk.expiredAt IS NULL OR uk.expiredAt > CURRENT_TIMESTAMP)
    """)
    List<UserKeysResponse> findValidKeysByUserId(
            @Param("userId") String userId,
            @Param("status") KeyStatus status
    );
//
//    @Modifying
//    @Transactional
//    @Query("""
//        UPDATE UserKeys uk
//        SET uk.status = :newStatus
//        WHERE uk.expiredAt IS NOT NULL
//          AND uk.expiredAt <= :now
//          AND uk.status <> :newStatus
//    """)
//    void expireOldKeys(
//            @Param("newStatus") KeyStatus newStatus,
//            @Param("now") LocalDateTime now
//    );
}
