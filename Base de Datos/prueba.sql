-- Esquema simple
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name='app') EXEC('CREATE SCHEMA app');
GO

CREATE TABLE app.Producto (
  ProductoId     int IDENTITY PRIMARY KEY,
  SKU            varchar(60) NOT NULL UNIQUE,
  Nombre         varchar(200) NOT NULL,
  Precio         decimal(19,2) NOT NULL CHECK (Precio >= 0),
  EsInventariable bit NOT NULL DEFAULT 1,
  Activo         bit NOT NULL DEFAULT 1
);

CREATE TABLE app.Cliente (
  ClienteId      int IDENTITY PRIMARY KEY,
  Nombre         varchar(200) NOT NULL,
  Email          varchar(200) NULL,
  Telefono       varchar(50)  NULL,
  Activo         bit NOT NULL DEFAULT 1
);

CREATE TABLE app.Almacen (
  AlmacenId      int IDENTITY PRIMARY KEY,
  Codigo         varchar(20) NOT NULL UNIQUE,
  Nombre         varchar(100) NOT NULL
);
IF NOT EXISTS (SELECT 1 FROM app.Almacen) INSERT app.Almacen(Codigo,Nombre) VALUES ('MAIN','Principal');

-- Ledger de stock (simple)
CREATE TABLE app.MovimientoStock (
  MovimientoId   bigint IDENTITY PRIMARY KEY,
  AlmacenId      int NOT NULL REFERENCES app.Almacen(AlmacenId),
  ProductoId     int NOT NULL REFERENCES app.Producto(ProductoId),
  Fecha          datetime2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  Cantidad       decimal(18,4) NOT NULL,   -- IN positivo, OUT negativo
  Origen         varchar(10) NOT NULL,     -- 'IN','OUT'
  Referencia     varchar(40) NULL          -- p.ej. Nº factura
);
CREATE INDEX IX_Stock_ProdAlm ON app.MovimientoStock(ProductoId, AlmacenId);

-- Ventas
CREATE TABLE app.Factura (
  FacturaId      bigint IDENTITY PRIMARY KEY,
  Numero         varchar(40) NOT NULL UNIQUE,
  ClienteId      int NOT NULL REFERENCES app.Cliente(ClienteId),
  Fecha          datetime2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  TotalBase      decimal(19,2) NOT NULL DEFAULT 0,
  TotalImpuesto  decimal(19,2) NOT NULL DEFAULT 0,
  TotalTotal     decimal(19,2) NOT NULL DEFAULT 0,
  Estado         varchar(12) NOT NULL DEFAULT 'ABIERTA'  -- ABIERTA/COBRADA/ANULADA
);

CREATE TABLE app.FacturaLinea (
  LineaId        bigint IDENTITY PRIMARY KEY,
  FacturaId      bigint NOT NULL REFERENCES app.Factura(FacturaId) ON DELETE CASCADE,
  NLinea         int NOT NULL,
  ProductoId     int NOT NULL REFERENCES app.Producto(ProductoId),
  Cantidad       decimal(18,4) NOT NULL CHECK (Cantidad > 0),
  PrecioUnit     decimal(19,2) NOT NULL CHECK (PrecioUnit >= 0),
  IVA_Pct        decimal(5,2) NOT NULL DEFAULT 21.00,
  CONSTRAINT UQ_FacturaLinea UNIQUE(FacturaId, NLinea)
);

-- Vistas rápidas
CREATE OR ALTER VIEW app.v_Stock AS
SELECT ProductoId, AlmacenId, SUM(Cantidad) AS Stock
FROM app.MovimientoStock
GROUP BY ProductoId, AlmacenId;

-- Semilla mínima
INSERT app.Producto(SKU,Nombre,Precio) VALUES ('SKU-001','Producto A',25.00), ('SKU-002','Servicio B',50.00);
INSERT app.Cliente(Nombre) VALUES ('Cliente Demo');

-- Ejemplo de venta con salida de stock
DECLARE @Fact bigint;
INSERT app.Factura(Numero,ClienteId) VALUES ('F-0001',1);
SET @Fact = SCOPE_IDENTITY();
INSERT app.FacturaLinea(FacturaId,NLinea,ProductoId,Cantidad,PrecioUnit,IVA_Pct)
VALUES (@Fact,1,1,2,25.00,21),(@Fact,2,2,1,50.00,21);

-- Movimiento OUT por productos inventariables (solo ProductoId=1)
INSERT app.MovimientoStock(AlmacenId,ProductoId,Cantidad,Origen,Referencia)
VALUES (1,1,-2,'OUT','F-0001');

-- Recalculo totales
;WITH L AS (
  SELECT Cantidad, PrecioUnit, IVA_Pct FROM app.FacturaLinea WHERE FacturaId=@Fact
)
UPDATE f SET
  TotalBase = (SELECT SUM(Cantidad*PrecioUnit) FROM L),
  TotalImpuesto = (SELECT SUM(Cantidad*PrecioUnit*IVA_Pct/100.0) FROM L),
  TotalTotal = (SELECT SUM(Cantidad*PrecioUnit*(1+IVA_Pct/100.0)) FROM L)
FROM app.Factura f WHERE f.FacturaId=@Fact;
GO
