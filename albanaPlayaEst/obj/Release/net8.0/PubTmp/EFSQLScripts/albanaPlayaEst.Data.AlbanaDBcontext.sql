CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Clientes` (
        `Cod_cliente` int NOT NULL AUTO_INCREMENT,
        `Nom_cliente` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Apell_cliente` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Tel_cliente` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Dni_cliente` varchar(12) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Clientes` PRIMARY KEY (`Cod_cliente`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Espacios` (
        `Cod_esp` int NOT NULL AUTO_INCREMENT,
        `Estad_esp` tinyint(1) NOT NULL,
        `Ubi_esp` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `Descr_esp` varchar(100) CHARACTER SET utf8mb4 NULL,
        `Cost_esp` decimal(65,30) NOT NULL,
        CONSTRAINT `PK_Espacios` PRIMARY KEY (`Cod_esp`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Metodpags` (
        `CodMetd` int NOT NULL AUTO_INCREMENT,
        `DescrMetd` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Metodpags` PRIMARY KEY (`CodMetd`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Tipovehics` (
        `CodTipV` int NOT NULL AUTO_INCREMENT,
        `DescrTipV` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Tipovehics` PRIMARY KEY (`CodTipV`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Usuarios` (
        `Correo` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Nombre` longtext CHARACTER SET utf8mb4 NULL,
        `Clave` longtext CHARACTER SET utf8mb4 NULL,
        `Roles` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Correo`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Pagos` (
        `cod_Pag` int NOT NULL AUTO_INCREMENT,
        `MontPag` decimal(65,30) NOT NULL,
        `CodMetd` int NOT NULL,
        `CodMetdNavigationCodMetd` int NULL,
        CONSTRAINT `PK_Pagos` PRIMARY KEY (`cod_Pag`),
        CONSTRAINT `FK_Pagos_Metodpags_CodMetdNavigationCodMetd` FOREIGN KEY (`CodMetdNavigationCodMetd`) REFERENCES `Metodpags` (`CodMetd`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Vehículos` (
        `CodV` int NOT NULL AUTO_INCREMENT,
        `MarcV` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `ModelV` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `ColorV` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        `PlacaV` varchar(8) CHARACTER SET utf8mb4 NOT NULL,
        `CodTipV` int NOT NULL,
        `Cod_cliente` int NOT NULL,
        `CodCliNavigationCod_cliente` int NULL,
        `CodTipVNavigationCodTipV` int NULL,
        CONSTRAINT `PK_Vehículos` PRIMARY KEY (`CodV`),
        CONSTRAINT `FK_Vehículos_Clientes_CodCliNavigationCod_cliente` FOREIGN KEY (`CodCliNavigationCod_cliente`) REFERENCES `Clientes` (`Cod_cliente`),
        CONSTRAINT `FK_Vehículos_Tipovehics_CodTipVNavigationCodTipV` FOREIGN KEY (`CodTipVNavigationCodTipV`) REFERENCES `Tipovehics` (`CodTipV`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE TABLE `Registros` (
        `CodReg` int NOT NULL AUTO_INCREMENT,
        `FechaEntrada` datetime(6) NULL,
        `FechaHoraSalida` datetime(6) NULL,
        `Cod_cliente` int NOT NULL,
        `CodV` int NOT NULL,
        `CodEsp` int NOT NULL,
        `cod_Pag` int NULL,
        `CodEspNavigationCod_esp` int NULL,
        `CodVNavigationCodV` int NULL,
        `CodPagNavigationcod_Pag` int NULL,
        `CodCliNavigationCod_cliente` int NULL,
        CONSTRAINT `PK_Registros` PRIMARY KEY (`CodReg`),
        CONSTRAINT `FK_Registros_Clientes_CodCliNavigationCod_cliente` FOREIGN KEY (`CodCliNavigationCod_cliente`) REFERENCES `Clientes` (`Cod_cliente`),
        CONSTRAINT `FK_Registros_Espacios_CodEspNavigationCod_esp` FOREIGN KEY (`CodEspNavigationCod_esp`) REFERENCES `Espacios` (`Cod_esp`),
        CONSTRAINT `FK_Registros_Pagos_CodPagNavigationcod_Pag` FOREIGN KEY (`CodPagNavigationcod_Pag`) REFERENCES `Pagos` (`cod_Pag`),
        CONSTRAINT `FK_Registros_Vehículos_CodVNavigationCodV` FOREIGN KEY (`CodVNavigationCodV`) REFERENCES `Vehículos` (`CodV`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Pagos_CodMetdNavigationCodMetd` ON `Pagos` (`CodMetdNavigationCodMetd`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Registros_CodCliNavigationCod_cliente` ON `Registros` (`CodCliNavigationCod_cliente`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Registros_CodEspNavigationCod_esp` ON `Registros` (`CodEspNavigationCod_esp`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Registros_CodPagNavigationcod_Pag` ON `Registros` (`CodPagNavigationcod_Pag`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Registros_CodVNavigationCodV` ON `Registros` (`CodVNavigationCodV`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Vehículos_CodCliNavigationCod_cliente` ON `Vehículos` (`CodCliNavigationCod_cliente`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    CREATE INDEX `IX_Vehículos_CodTipVNavigationCodTipV` ON `Vehículos` (`CodTipVNavigationCodTipV`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241116011149_Mi1') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241116011149_Mi1', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Registros` DROP FOREIGN KEY `FK_Registros_Clientes_CodCliNavigationCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Registros` DROP COLUMN `Cod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Registros` RENAME COLUMN `CodCliNavigationCod_cliente` TO `ClienteCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Registros` RENAME INDEX `IX_Registros_CodCliNavigationCod_cliente` TO `IX_Registros_ClienteCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Clientes` MODIFY COLUMN `Dni_cliente` varchar(8) CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Clientes_ClienteCod_cliente` FOREIGN KEY (`ClienteCod_cliente`) REFERENCES `Clientes` (`Cod_cliente`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117153720_Mi2') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241117153720_Mi2', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117233359_Mi3') THEN

    ALTER TABLE `Pagos` MODIFY COLUMN `MontPag` decimal(65,30) NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241117233359_Mi3') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241117233359_Mi3', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` DROP FOREIGN KEY `FK_Registros_Espacios_CodEspNavigationCod_esp`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` DROP FOREIGN KEY `FK_Registros_Pagos_CodPagNavigationcod_Pag`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` DROP FOREIGN KEY `FK_Registros_Vehículos_CodVNavigationCodV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` DROP FOREIGN KEY `FK_Vehículos_Clientes_CodCliNavigationCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` DROP FOREIGN KEY `FK_Vehículos_Tipovehics_CodTipVNavigationCodTipV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` RENAME COLUMN `CodTipVNavigationCodTipV` TO `TipovehicCodTipV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` RENAME COLUMN `CodCliNavigationCod_cliente` TO `ClienteCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` RENAME INDEX `IX_Vehículos_CodTipVNavigationCodTipV` TO `IX_Vehículos_TipovehicCodTipV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` RENAME INDEX `IX_Vehículos_CodCliNavigationCod_cliente` TO `IX_Vehículos_ClienteCod_cliente`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME COLUMN `CodVNavigationCodV` TO `VehículoCodV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME COLUMN `CodPagNavigationcod_Pag` TO `Pagocod_Pag`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME COLUMN `CodEspNavigationCod_esp` TO `EspacioCod_esp`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME INDEX `IX_Registros_CodVNavigationCodV` TO `IX_Registros_VehículoCodV`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME INDEX `IX_Registros_CodPagNavigationcod_Pag` TO `IX_Registros_Pagocod_Pag`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` RENAME INDEX `IX_Registros_CodEspNavigationCod_esp` TO `IX_Registros_EspacioCod_esp`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Espacios_EspacioCod_esp` FOREIGN KEY (`EspacioCod_esp`) REFERENCES `Espacios` (`Cod_esp`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Pagos_Pagocod_Pag` FOREIGN KEY (`Pagocod_Pag`) REFERENCES `Pagos` (`cod_Pag`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Vehículos_VehículoCodV` FOREIGN KEY (`VehículoCodV`) REFERENCES `Vehículos` (`CodV`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` ADD CONSTRAINT `FK_Vehículos_Clientes_ClienteCod_cliente` FOREIGN KEY (`ClienteCod_cliente`) REFERENCES `Clientes` (`Cod_cliente`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    ALTER TABLE `Vehículos` ADD CONSTRAINT `FK_Vehículos_Tipovehics_TipovehicCodTipV` FOREIGN KEY (`TipovehicCodTipV`) REFERENCES `Tipovehics` (`CodTipV`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118021843_Mi4') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241118021843_Mi4', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Clientes` MODIFY COLUMN `Tel_cliente` varchar(15) CHARACTER SET utf8mb4 NOT NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Pagos` ADD CONSTRAINT `FK_Pagos_Metodpags_CodMetd` FOREIGN KEY (`CodMetd`) REFERENCES `Metodpags` (`CodMetd`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Espacios_CodEsp` FOREIGN KEY (`CodEsp`) REFERENCES `Espacios` (`Cod_esp`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Pagos_cod_Pag` FOREIGN KEY (`cod_Pag`) REFERENCES `Pagos` (`cod_Pag`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Registros` ADD CONSTRAINT `FK_Registros_Vehículos_CodV` FOREIGN KEY (`CodV`) REFERENCES `Vehículos` (`CodV`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Vehículos` ADD CONSTRAINT `FK_Vehículos_Clientes_Cod_cliente` FOREIGN KEY (`Cod_cliente`) REFERENCES `Clientes` (`Cod_cliente`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    ALTER TABLE `Vehículos` ADD CONSTRAINT `FK_Vehículos_Tipovehics_CodTipV` FOREIGN KEY (`CodTipV`) REFERENCES `Tipovehics` (`CodTipV`) ON DELETE CASCADE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241118024000_Mi5') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241118024000_Mi5', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241119165324_Mi6') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241119165324_Mi6', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241119165548_Mi7') THEN

    CREATE TABLE `Usuarios` (
        `Id_user` int NOT NULL AUTO_INCREMENT,
        `Nombre` longtext CHARACTER SET utf8mb4 NULL,
        `Correo` longtext CHARACTER SET utf8mb4 NULL,
        `Clave` longtext CHARACTER SET utf8mb4 NULL,
        `Roles` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id_user`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241119165548_Mi7') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241119165548_Mi7', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241123234439_Mi8') THEN

    ALTER TABLE `Usuarios` RENAME COLUMN `Roles` TO `RolesJson`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241123234439_Mi8') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241123234439_Mi8', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241124013107_Mi9') THEN

    ALTER TABLE `Usuarios` ADD `FotoPerfil` longtext CHARACTER SET utf8mb4 NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241124013107_Mi9') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241124013107_Mi9', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241125004458_Mi10') THEN

    ALTER TABLE `Pagos` ADD `EsPagoRecurrente` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241125004458_Mi10') THEN

    ALTER TABLE `Pagos` ADD `FechaFin` datetime(6) NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241125004458_Mi10') THEN

    ALTER TABLE `Pagos` ADD `FechaInicio` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241125004458_Mi10') THEN

    ALTER TABLE `Pagos` ADD `MesesPagados` int NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241125004458_Mi10') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241125004458_Mi10', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241128150322_Mi11') THEN

    ALTER TABLE `Pagos` ADD `Estado` tinyint(1) NOT NULL DEFAULT FALSE;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241128150322_Mi11') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241128150322_Mi11', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241201024321_Mi12') THEN

    ALTER TABLE `Pagos` DROP COLUMN `MesesPagados`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20241201024321_Mi12') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20241201024321_Mi12', '8.0.11');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

