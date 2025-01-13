--- Initial Data Input ---
DROP DATABASE IF EXISTS jm_ictprg432;
CREATE DATABASE IF NOT EXISTS jm_ictprg432;
USE jm_ictprg432;
DROP TABLE IF EXISTS employees;

CREATE TABLE `employees` (
    `id` bigint UNSIGNED NOT NULL AUTO_INCREMENT,
    `given_name` varchar(64),
    `family_name` varchar(64) NOT NULL,
    `date_of_birth` date DEFAULT '1970-01-01',
    `gender_identity` char(1),
    `gross_salary` int DEFAULT '0',
    `supervisor_id` bigint DEFAULT '0',
    `branch_id` bigint DEFAULT '0',
    `created_at` timestamp DEFAULT '2022-07-01',
    `updated_at` timestamp ON UPDATE CURRENT_TIMESTAMP, 
    PRIMARY KEY (id)
);

INSERT INTO `employees`  
	(`date_of_birth`, `id`, `family_name`, `branch_id`,`supervisor_id`, `given_name`, `gross_salary`, `gender_identity`)
VALUES
	('1967-05-11', 101, 'Levinson', 1, 100, 'Jan', 110000, 'F');

INSERT INTO `employees` 
	(`date_of_birth`, `id`, `family_name`, `branch_id`, `supervisor_id`, `given_name`, `gross_salary`, `gender_identity`) 
VALUES
	('1964-03-15', 102, 'Scott', 2, 100, 'Michael', 75000, 'O'),
	('1971-06-25', 103, 'Martin', 2, 102, 'Angela', 63000, 'F'),
	('1980-02-05', 104, 'Kapoor', 2, 102, 'Kelly', 55000, 'O'),
	('1958-02-19', 105, 'Hudson', 2, 102, 'Stanley', 69000, 'M'),
	('1969-09-05', 106, 'Porter', 3, 100, 'Josh', 78000, 'M'),
	('1973-07-22', 107, 'Bernard', 3, 106, 'Andy', 65000, 'M'),
	('1978-10-01', 108, 'Halpert', 3, 106, 'Jen', 71000, 'F');


CREATE TABLE `branches` (
    `id`                  BIGINT        UNSIGNED NOT NULL AUTO_INCREMENT,
    `branch_name`         VARCHAR(64)   NOT NULL  DEFAULT 'ERROR'  UNIQUE,
    `manager_id`          BIGINT        UNSIGNED  DEFAULT '0',
    `manager_started_at`  DATE          NOT NULL  DEFAULT '1900-01-01',
    `created_at`          TIMESTAMP     NOT NULL DEFAULT NOW(),
    `updated_at`          TIMESTAMP     ON UPDATE CURRENT_TIMESTAMP, 
    PRIMARY KEY (id)
);

INSERT INTO branches(`id`,`branch_name`,`manager_id`, `manager_started_at`)
VALUES
    (1, 'Corporate',  100, "2006-02-09"),
    (2, 'Scranton',   102, "1992-04-06"),
    (3, 'Stamford',   106, "1998-02-13");


CREATE TABLE `clients` (
    `id`               BIGINT        UNSIGNED  NOT NULL AUTO_INCREMENT,
    `client_name`      VARCHAR(64)   NOT NULL,
    `branch_id`        BIGINT        UNSIGNED,
    `created_at`       TIMESTAMP     NOT NULL DEFAULT NOW(),
    `updated_at`       TIMESTAMP     ON UPDATE CURRENT_TIMESTAMP, 
    PRIMARY KEY (id)
);

INSERT INTO clients(`id`,`client_name`,`branch_id`)
VALUES
    (400, 'Dunmore Hoghschool', 2),
    (401, 'Lackawana Country', 2),
    (402, 'FedEx', 3),
    (403, 'John Daly Law, LLC', 3),
    (404, 'Scranton Whitepages', 2),
    (405, 'Times Newspaper', 3),
    (406, 'FedEx', 2);


CREATE TABLE `working_with` (
    `employee_id`   BIGINT     UNSIGNED  NOT NULL,
    `client_id`     BIGINT     UNSIGNED  NOT NULL,
    `total_sales`   BIGINT     UNSIGNED  NOT NULL  DEFAULT 0,
    `created_at`    TIMESTAMP  NOT NULL  DEFAULT NOW(),
    `updated_at`    TIMESTAMP  ON UPDATE CURRENT_TIMESTAMP, 
    PRIMARY KEY (employee_id,client_id)
);

INSERT INTO working_with(employee_id,client_id,total_sales)
VALUES
    (105, 400, 55000),
    (102, 401, 267000),
    (108, 402, 22500),
    (107, 403, 5000),
    (108, 403, 12000),
    (105, 404, 33000),
    (107, 405, 26000),
    (102, 406, 15000),
    (105, 406, 130000);

CREATE TABLE `branch_supplier` (
    `branch_id`           BIGINT        UNSIGNED NOT NULL,
    `supplier_name`       VARCHAR(64)   NOT NULL,
    `product_supplied`    VARCHAR(64)   NOT NULL  DEFAULT 'Not classifed',
    `created_at`          TIMESTAMP     NOT NULL DEFAULT NOW(),
    `updated_at`          TIMESTAMP     ON UPDATE CURRENT_TIMESTAMP, 
    PRIMARY KEY (branch_id,supplier_name)
);

INSERT INTO branch_supplier(`branch_id`,`supplier_name`,`product_supplied`)
VALUES
    (2, 'Hammer Mill', 'Paper'),
    (2, 'Uni-Ball', 'Writing Instruments'),
    (3, 'Patriot Paper', 'Paper'),
    (2, 'J. T. Forms & Labels', 'Custom Forms'),
    (3, 'Uni-Ball', 'Writing Instruments'),
    (3, 'Hammer Mill', 'Paper'),
    (3, 'Stamford Labels', 'Custom Forms');


