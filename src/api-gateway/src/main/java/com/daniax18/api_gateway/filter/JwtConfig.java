package com.daniax18.api_gateway.filter;

import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.ReactiveSecurityContextHolder;
import org.springframework.web.server.ServerWebExchange;
import org.springframework.web.server.WebFilter;
import org.springframework.web.server.WebFilterChain;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.Map;

public class JwtConfig implements WebFilter {
    private final JwtUtils jwtUtils;

    public JwtConfig(JwtUtils jwtUtils){
        this.jwtUtils = jwtUtils;
    }

    /**
     * Filtre de sécurité chargé d'intercepter chaque requête HTTP afin de vérifier
     * la présence et la validité d'un token JWT dans l'en-tête Authorization.
     *
     * Si le token est valide, l'utilisateur est authentifié et ajouté au contexte
     * de sécurité réactif. Sinon, la requête est rejetée avec un statut HTTP 401.
     *
     * @param exchange représente la requête et la réponse HTTP en cours
     * @param chain permet de continuer la chaîne de filtres
     * @return Mono<Void> indiquant la fin du traitement du filtre
     */
    @Override
    public Mono<Void> filter(ServerWebExchange exchange, WebFilterChain chain) {
        String authHeader = exchange.getRequest()
                .getHeaders()
                .getFirst(HttpHeaders.AUTHORIZATION);

        if(authHeader != null && authHeader.startsWith("Bearer ")){
            String jwt = authHeader.substring(7);
            Map<String, Object> tokenData = jwtUtils.tokenData(jwt);

            boolean isExpired = (Boolean)tokenData.get("isTokenExpired");

            if(isExpired){
                exchange.getResponse().setStatusCode(HttpStatus.UNAUTHORIZED);
                return exchange.getResponse().setComplete();
            }

            String userName = (String) tokenData.get("userName");
            String role = (String) tokenData.get("role");

            // Création d'un objet Authentication pour Spring Security
            Authentication authentication = new UsernamePasswordAuthenticationToken(
                    userName,
                    jwt,
                    List.of(new SimpleGrantedAuthority(role))
            );

            // Ajoute l'utilisateur authentifié dans le contexte de sécurité réactif
            return  chain.filter(exchange)
                    .contextWrite(ReactiveSecurityContextHolder.withAuthentication(authentication));
        }

        return chain.filter(exchange);
    }
}
