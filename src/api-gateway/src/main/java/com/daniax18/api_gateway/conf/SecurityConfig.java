package com.daniax18.api_gateway.conf;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.reactive.EnableWebFluxSecurity;
import org.springframework.security.config.web.server.ServerHttpSecurity;
import org.springframework.security.web.server.SecurityWebFilterChain;

import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.reactive.UrlBasedCorsConfigurationSource;
import org.springframework.web.cors.reactive.CorsConfigurationSource;

import java.util.List;

/**
 * Classe de configuration de la sécurité de l'application.
 *
 * Cette classe définit les règles de sécurité appliquées aux requêtes HTTP,
 * notamment l'authentification, les routes accessibles sans authentification
 * et la configuration CORS.
 */
@Configuration
@EnableWebFluxSecurity
public class SecurityConfig {

    @Bean
    public SecurityWebFilterChain securityWebFilterChain(ServerHttpSecurity http){
        return http.
                csrf(ServerHttpSecurity.CsrfSpec::disable)                                          // Désactive la protection CSRF (utile pour les API REST stateless)
                .cors(
                        cors -> cors.configurationSource(corsConfigurationSource())       // Active et configure CORS
                )
                .authorizeExchange(exchange -> exchange                       // Définit les règles d'autorisation des requêtes
                        .pathMatchers("/api/user/login").permitAll()
                        .anyExchange().authenticated()
                )
                .build();
    }

    @Bean
    public CorsConfigurationSource corsConfigurationSource() {

        CorsConfiguration config = new CorsConfiguration();
        config.setAllowedOrigins(List.of("http://localhost:3000"));
        config.setAllowedMethods(List.of("GET","POST","PUT","DELETE","OPTIONS"));
        config.setAllowedHeaders(List.of("*"));
        config.setMaxAge(3600L);

        // Associe la configuration CORS à toutes les routes de l'API
        UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        source.registerCorsConfiguration("/**", config);

        return source;
    }
}
