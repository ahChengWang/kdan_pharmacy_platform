CREATE TABLE IF NOT EXISTS `mask` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(50) NOT NULL COMMENT '口罩名稱',
  `CreateUser` VARCHAR(50) NOT NULL DEFAULT '',
  `CreateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateUser` VARCHAR(50) NOT NULL DEFAULT '',
  `UpdateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = '口罩基本資料';