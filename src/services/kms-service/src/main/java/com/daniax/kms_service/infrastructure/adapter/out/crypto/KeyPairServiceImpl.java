package com.daniax.kms_service.infrastructure.adapter.out.crypto;

import com.daniax.kms_service.application.port.out.IKeyPairService;
import com.daniax.kms_service.application.utils.AlgorithmType;
import org.springframework.stereotype.Component;

import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.util.Base64;

@Component
public class KeyPairServiceImpl implements IKeyPairService {
    @Override
    public String[] generateKeys(AlgorithmType algorithmType) throws NoSuchAlgorithmException {
        try {
            KeyPairGenerator keyGen = KeyPairGenerator.getInstance(algorithmType.getValue());
            keyGen.initialize(2048, new SecureRandom());
            KeyPair keyPair = keyGen.generateKeyPair();

            String publicKey = Base64.getEncoder().encodeToString(
                    keyPair.getPublic().getEncoded()
            );
            String privateKey = Base64.getEncoder().encodeToString(
                    keyPair.getPrivate().getEncoded()
            );

            return new String[]{publicKey, privateKey};
        } catch (NoSuchAlgorithmException e) {
            throw new NoSuchAlgorithmException("Error on generating keys" + e.getMessage());
        }
    }
}
