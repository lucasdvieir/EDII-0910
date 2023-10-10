using System;
using System.Collections.Generic;
using System.Linq;

class Lote
{
    public int id { get; set; }
    public int qtde { get; set; }
    public DateTime venc { get; set; }

    public Lote()
    {
    }

    public Lote(int id, int qtde, DateTime venc)
    {
        this.id = id;
        this.qtde = qtde;
        this.venc = venc;
    }

    public override string ToString()
    {
        return "Id: " + id + ","+ "Quant.: " + qtde + "," + "Vencimento: " + venc.ToString("dd-MM-yyyy");
    }
}

class Medicamento
{
    public int id { get; set; }
    public string nome { get; set; }
    public string laboratorio { get; set; }
    public Queue<Lote> lotes { get; set; }

    public Medicamento()
    {
        lotes = new Queue<Lote>();
    }

    public Medicamento(int id, string nome, string laboratorio)
    {
        this.id = id;
        this.nome = nome;
        this.laboratorio = laboratorio;
        lotes = new Queue<Lote>();
    }

    public int qtdeDisponivel()
    {
        return lotes.Sum(lote => lote.qtde);
    }

    public void comprar(Lote lote)
    {
        lotes.Enqueue(lote);
    }

    public bool vender(int qtde)
    {
        while (qtde > 0 && lotes.Count > 0)
        {
            var lote = lotes.Peek();
            if (qtde >= lote.qtde)
            {
                qtde -= lote.qtde;
                lotes.Dequeue();
            }
            else
            {
                lote.qtde -= qtde;
                qtde = 0;
            }
        }

        return qtde == 0;
    }

    public override string ToString()
    {
        return "ID:"+ id + ", " +"Nome: " + nome + ", " + "Laboratório: " + laboratorio + ", " +"Quantidadae: " + qtdeDisponivel();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return id == ((Medicamento)obj).id;
    }
}

class Medicamentos
{
    public List<Medicamento> listaMedicamentos { get; set; }

    public Medicamentos()
    {
        listaMedicamentos = new List<Medicamento>();
    }

    public void adicionar(Medicamento medicamento)
    {
        listaMedicamentos.Add(medicamento);
    }

    public bool deletar(Medicamento medicamento)
    {
        if (medicamento.qtdeDisponivel() == 0)
        {
            return listaMedicamentos.Remove(medicamento);
        }
        return false;
    }

    public Medicamento pesquisar(int id)
    {
        return listaMedicamentos.FirstOrDefault(med => med.id == id) ?? new Medicamento();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Medicamentos medicamentos = new Medicamentos();
        int option;

        do
        {
            Console.WriteLine("Selecione uma opção:");
            Console.WriteLine("0. Finalizar processo");
            Console.WriteLine("1. Cadastrar medicamento");
            Console.WriteLine("2. Consultar medicamento (sintético)");
            Console.WriteLine("3. Consultar medicamento (analítico)");
            Console.WriteLine("4. Comprar medicamento (cadastrar lote)");
            Console.WriteLine("5. Vender medicamento (abater do lote mais antigo)");
            Console.WriteLine("6. Listar medicamentos (informando dados sintéticos)");
            if (int.TryParse(Console.ReadLine(), out option))
            {
                switch (option)
                {
                    case 0:
                        Console.WriteLine("Processo finalizado.");
                        break;
                    case 1:
                        // Cadastro de medicamento
                        Console.Write("Informe o ID do medicamento: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Informe o nome do medicamento: ");
                        string nome = Console.ReadLine();
                        Console.Write("Informe o laboratório do medicamento: ");
                        string laboratorio = Console.ReadLine();
                        Medicamento novoMedicamento = new Medicamento(id, nome, laboratorio);
                        medicamentos.adicionar(novoMedicamento);
                        break;
                    case 2:
                        // Consulta de medicamento (sintético)
                        Console.Write("Informe o ID do medicamento: ");
                        int idConsultaSintetica = int.Parse(Console.ReadLine());
                        Medicamento medConsultaSintetica = medicamentos.pesquisar(idConsultaSintetica);
                        if (medConsultaSintetica.id != 0)
                        {
                            Console.WriteLine($"Dados do medicamento: {medConsultaSintetica}");
                        }
                        else
                        {
                            Console.WriteLine("Medicamento não encontrado.");
                        }
                        break;
                    case 3:
                        // Consulta de medicamento (analítico)
                        Console.Write("Informe o ID do medicamento: ");
                        int idConsultaAnalitica = int.Parse(Console.ReadLine());
                        Medicamento medConsultaAnalitica = medicamentos.pesquisar(idConsultaAnalitica);
                        if (medConsultaAnalitica.id != 0)
                        {
                            Console.WriteLine($"Dados do medicamento: {medConsultaAnalitica}");
                            Console.WriteLine("Dados dos lotes:");
                            foreach (var lote in medConsultaAnalitica.lotes)
                            {
                                Console.WriteLine($"ID: {lote.id}, Quantidade: {lote.qtde}, Vencimento: {lote.venc.ToString("dd-MM-yyyy")}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Medicamento não encontrado.");
                        }
                        break;
                    case 4:
                        // Comprar medicamento (cadastrar lote)
                        Console.Write("Informe o ID do medicamento: ");
                        int idCompra = int.Parse(Console.ReadLine());
                        Medicamento medCompra = medicamentos.pesquisar(idCompra);
                        if (medCompra.id != 0)
                        {
                            Console.Write("Informe o ID do lote: ");
                            int idLoteCompra = int.Parse(Console.ReadLine());
                            Console.Write("Informe a quantidade: ");
                            int qtdeCompra = int.Parse(Console.ReadLine());
                            Console.Write("Informe a data de vencimento (dd-mm-aaaa): ");
                            DateTime vencimentoLoteCompra = DateTime.ParseExact(Console.ReadLine(), "dd-MM-yyyy", null);

                            Lote loteCompra = new Lote(idLoteCompra, qtdeCompra, vencimentoLoteCompra);
                            medCompra.comprar(loteCompra);
                        }
                        else
                        {
                            Console.WriteLine("Medicamento não encontrado.");
                        }
                        break;
                    case 5:
                        // Vender medicamento (abater do lote mais antigo)
                        Console.Write("Informe o ID do medicamento: ");
                        int idVenda = int.Parse(Console.ReadLine());
                        Medicamento medVenda = medicamentos.pesquisar(idVenda);
                        if (medVenda.id != 0)
                        {
                            Console.Write("Informe a quantidade: ");
                            int qtdeVenda = int.Parse(Console.ReadLine());
                            bool sucessoVenda = medVenda.vender(qtdeVenda);
                            if (sucessoVenda)
                            {
                                Console.WriteLine("Venda realizada com sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Quantidade disponível insuficiente para venda.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Medicamento não encontrado.");
                        }
                        break;
                    case 6:
                        // Listar medicamentos (informando dados sintéticos)
                        Console.WriteLine("Lista de medicamentos:");
                        foreach (var med in medicamentos.listaMedicamentos)
                        {
                            Console.WriteLine($"Dados do medicamento: {med}");
                        }
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }

        } while (option != 0);
    }
}
