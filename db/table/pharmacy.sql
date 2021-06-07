CREATE TABLE IF NOT EXISTS `pharmacy` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) CHARACTER SET 'utf8' NOT NULL COMMENT '藥局名稱',
  `Status` INT(2) NOT NULL DEFAULT '1' COMMENT '藥局狀態 0:停業 1:正常營業 2:暫時歇業',
  `CreateUser` VARCHAR(50) NOT NULL DEFAULT '',
  `CreateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdateUser` VARCHAR(50) NOT NULL DEFAULT '',
  `UpdateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_unicode_ci
COMMENT = '藥局基本資料';