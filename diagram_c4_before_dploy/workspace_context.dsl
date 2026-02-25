workspace "Order transactions Plateform" "C4 content Diagram" {
    model {
        user = person "User" "A user of the system"

        transactionPlateform = softwareSystem "Order Plateform" "Allows users to place orders and securize their transactions"  "Internal" 
        
        minio = softwareSystem "MinIO Object Storage" "Video storage" "External"

        RabbitMQ = softwareSystem "RabbitMQ" "Message broker" "External"

        user -> transactionPlateform "Uses"
        transactionPlateform -> minio "Stores videos in"
        transactionPlateform -> RabbitMQ "Sends messages to"
    }

    views {
        systemContext transactionPlateform {
            include *
            //autolayout lr
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
        }

        theme default
    }
}