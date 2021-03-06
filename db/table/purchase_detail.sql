CREATE TABLE `purchase_detail` (
	`Id` INT(10) NOT NULL AUTO_INCREMENT,
	`PurchaseId` INT(10) NOT NULL,
	`PharmacyProductId` INT(10) NOT NULL,
	`DetailStatus` INT(10) NOT NULL COMMENT '子項處理狀態',
	`Quantity` INT(10) NOT NULL COMMENT '數量',
	`Amount` DECIMAL(20,4) NOT NULL DEFAULT '0.0000' COMMENT '金額',
	`CreateUser` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_unicode_ci',
	`CreateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
	`UpdateUser` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8mb4_unicode_ci',
	`UpdateTime` DATETIME NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`Id`) USING BTREE
)
COMMENT='客戶訂單資料'
COLLATE='utf8mb4_unicode_ci'
ENGINE=InnoDB
;
