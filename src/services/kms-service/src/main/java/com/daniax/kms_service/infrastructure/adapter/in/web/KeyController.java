package com.daniax.kms_service.infrastructure.adapter.in.web;

import com.daniax.kms_service.application.port.in.IGenerateKeyUseCase;
import com.daniax.kms_service.application.port.in.IGetKeysByUserIdUseCase;
import com.daniax.kms_service.application.port.in.IGetKeysValidByUserIdUseCase;
import com.daniax.kms_service.application.port.in.IRevokeKeyUseCase;
import com.daniax.kms_service.application.utils.GeneratedKeyRequest;
import com.daniax.kms_service.application.utils.GeneratedKeyResult;
import com.daniax.kms_service.application.utils.RevokedKeyResult;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;

import java.nio.charset.StandardCharsets;

@RestController
@RequestMapping("/api/keys")
public class KeyController {
    private final IGenerateKeyUseCase generateKeyUseCase;
    private final IGetKeysByUserIdUseCase getKeysByUserIdUseCase;
    private final IGetKeysValidByUserIdUseCase getKeysValidByUserIdUseCase;
    private final IRevokeKeyUseCase revokeKeyUseCase;

    public KeyController(
            IGenerateKeyUseCase generateKeyUseCase,
            IGetKeysByUserIdUseCase getKeysByUserIdUseCase,
            IGetKeysValidByUserIdUseCase getKeysValidByUserIdUseCase,
            IRevokeKeyUseCase revokeKeyUseCase
    ) {
        this.generateKeyUseCase = generateKeyUseCase;
        this.getKeysByUserIdUseCase = getKeysByUserIdUseCase;
        this.getKeysValidByUserIdUseCase = getKeysValidByUserIdUseCase;
        this.revokeKeyUseCase = revokeKeyUseCase;
    }

    @PostMapping("/generate")
    public ResponseEntity<Object> generateKeyPair(@RequestBody GeneratedKeyRequest request){
        try {
            GeneratedKeyResult result = generateKeyUseCase.generateKeys(request);
            String keyName = request.keyName();
            String sk = result.sk();

            // Contenu du fichier
            byte[] data = sk.getBytes(StandardCharsets.UTF_8);
            ByteArrayResource resource = new ByteArrayResource(data);

            return ResponseEntity.ok()
                    .header(HttpHeaders.CONTENT_DISPOSITION,
                            "attachment; filename=\"" + keyName + ".pem\"")
                    .contentType(MediaType.APPLICATION_OCTET_STREAM)
                    .contentLength(data.length)
                    .cacheControl(CacheControl.noStore())
                    .body(resource);

        } catch (Exception e) {
            return new ResponseEntity<>(
                    e.getMessage(),
                    HttpStatus.BAD_REQUEST
            );
        }
    }

    @PostMapping("/revoke")
    public ResponseEntity<String> revokeKey(@RequestParam Long id){
        try {
            RevokedKeyResult keyName = revokeKeyUseCase.revokeKey(id);
            String result = keyName.keyName() + " revoked successfull !";
            return new ResponseEntity<>(result, HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(
                    e.getMessage(),
                    HttpStatus.BAD_REQUEST
            );
        }
    }

    @GetMapping("/getkeys")
    public ResponseEntity<Object> getUserKeys(@RequestParam String userId){
        try {
            return new ResponseEntity<>(getKeysByUserIdUseCase.getAllKeysByUserId(userId), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(
                    e.getMessage(),
                    HttpStatus.BAD_REQUEST
            );
        }
    }

    @GetMapping("/getvalidkeys")
    public ResponseEntity<Object> getValidUserKeys(@RequestParam String userId){
        try {
            return new ResponseEntity<>(getKeysValidByUserIdUseCase.getKeysValidByUserId(userId), HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(
                    e.getMessage(),
                    HttpStatus.BAD_REQUEST
            );
        }
    }
}
