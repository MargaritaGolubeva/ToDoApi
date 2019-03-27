CREATE TABLE [dbo].[TodoItems] (
    [ID]    NVARCHAR (450) NOT NULL,
    [Name]  NVARCHAR (MAX) NULL,
    [Notes] NVARCHAR (MAX) NULL,
    [Done]  BIT            NULL,
    CONSTRAINT [PK_TodoItems] PRIMARY KEY CLUSTERED ([ID] ASC)
);

