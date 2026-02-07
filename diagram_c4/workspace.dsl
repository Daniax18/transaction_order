workspace "Order transactions Plateform" "C4 content Diagram" {
    model {
        user = person "User" "A user of the system"

        transactionPlateform = softwareSystem "Order Plateform" "Allows users to place orders and securize their transactions" "Internal" {
            // Zoom in the container of the transaction plateform : rendu/container.png
            frontend = container "Frontend Web" "Frontend app that provides user to create/verify transactions" "Next.js"  "front"

            apiGateway = container "API Gateway" "Single entry point - Routing." "REST API" 

            authService = container "Auth Service" "User management using Keycloack server." "REST API" "csharp"

            userDb = container "User Database" "Storage for users and table keys." "PostgreSQL" "postgres"

            transactionService = container "Metadata Service" "Transaction handling, upload, signatures." "REST API" "csharp"

            transactionDb = container "Transaction Database" "Storage for transaction metadata and signatures." "PostgreSQL" "postgres"

            notificationService = container "Notification service" "Allows users to get notification on transaction statut changed" "REST API" "csharp"

            notificationDb = container "Notification Database" "Storage for notifications." "MongoDB" "mongo"

            auditService = container "Audit Service" "Logs of transaction platform activities." "REST API" "java"
        }
        
        minio = softwareSystem "MinIO Object Storage" "Video storage" "External, minIo"

        RabbitMQ = softwareSystem "RabbitMQ" "Message broker" "External,RabbitMQ"

        // Relationships between containers
        user -> frontend "Users create/verify transactions using"
        apiGateway -> authService "Authenticate or create account using"
        frontend -> apiGateway "Proceeds API calls using"
        apiGateway -> transactionService "Create/verify transaction order using"
        apiGateway -> notificationService "Get notifications using"
        apiGateway -> auditService "Get Logs actions using"
        authService -> RabbitMQ "send user action events using"
        transactionService -> RabbitMQ "send transaction events using"
        RabbitMQ -> notificationService "send transaction notification events to"
        RabbitMQ -> auditService "send transaction/auth log events to"

        // Relations between service and database
        authService -> userDb "Reads from and writes user data using"
        transactionService -> transactionDb "Reads from and writes transaction metadata using"
        notificationService -> notificationDb "Reads from and writes notification data using"

        transactionService -> minio "Store and retrieve videos using"
    }

    views {

        container transactionPlateform {
            include *
            // autolayout lr    // => In order to personalize the vieew, we can comment this line and modify the position as like as we want
        }
        styles {
            element "Person" {
                shape person
                background #08427b
                color #ffffff
            }

            element "Internal" {
                background #1168bd
                color #ffffff
            }

            element "External" {
                background #eeeeee
                color #000000
                border dashed
            }

            element "front" {
                icon "./icons/next.png"
            }

            element "java" {
                icon "./icons/spring.png"
            }

            element "csharp" {
                icon "./icons/net.png"
            }

            element "storage" {
                icon "./icons/miniIO.jpg"
            }

            element "mongo" {
                shape Cylinder
                background #999999
                color #ffffff
                icon "./icons/mongodb.png"
            }

            element "postgres" {
                shape Cylinder
                background #999999
                color #ffffff
                icon "./icons/postgres.png"
            }

            element "minIo" {
                icon "./icons/miniIO.jpg"
            }

            element "RabbitMQ" {
                icon "./icons/rabbit.png"
            }
        }

        theme default
    }
}