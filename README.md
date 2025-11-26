# Integrantes do Grupo:
- Carolina Machado RM: 552925
- Felipe Nakama RM: 552821
- Micael Azarias RM: 552699
- Nathan Uflacker RM: 553264

---

# CareTrack — Registro de Hábitos de Saúde

Aplicação **console em C#** desenvolvida para registrar e acompanhar atividades de saúde e bem-estar.  
O objetivo é incentivar hábitos saudáveis com simplicidade, permitindo que o usuário registre dados, visualize estatísticas e acompanhe sua evolução.

## Funcionalidades

- **Adicionar registro**  
  O usuário informa:
  - Tipo de atividade (água, exposição ao sol, alongamentos, qualidade do sono)  
  - Data (ou usa a data atual)  
  - Valor (litros, minutos, quantidade ou avaliação subjetiva)  

- **Listar registros**  
  Exibe todos os registros cadastrados em formato tabular, com ID, tipo, data e valor.

- **Exibir estatísticas**  
  Calcula e apresenta:
  - Soma total e média dos valores por tipo de atividade  
  - Distribuição da qualidade do sono (Bom, Médio, Ruim)  

- **Sair**  
  Encerra o programa com mensagem de despedida.

## Tecnologias Utilizadas

- Linguagem: **C#**  
- Estruturas: `enum`, `struct`, arrays dinâmicos (`Array.Resize`)  
- Validação de entradas com `TryParse` e tratamento de erros (`try/catch`)  
- Interface de texto com **cores e bordas ASCII** para melhor experiência do usuário  

## Estrutura do Código

- `ActivityType` → Enum com os tipos de atividade  
- `ActivityRecord` → Struct que representa cada registro  
- `Program` → Classe principal com métodos:
  - `AddRecord()` → Adicionar registro  
  - `ListRecords()` → Listar registros  
  - `ShowStatistics()` → Exibir estatísticas  
  - `GetValidatedDate()`, `GetValidatedDouble()`, `GetValidatedInt()` → Validação de entradas  
  - Métodos auxiliares para formatação e feedback  

## Como Executar

1. **Clonar ou baixar o projeto**  
   git clone https://github.com/seu-repositorio/caretrack.git
   cd caretrack
2. **Compilar o projeto**
   dotnet build
4. **Executar o programa**
   dotnet run

