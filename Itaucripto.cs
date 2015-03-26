using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itaucripto_NET {
	public class Itaucripto {
		private int[] sbox;
		private int[] key;
		private string codEmp;
		private string numPed;
		private string tipPag;
		private string CHAVE_ITAU = "SEGUNDA12345ITAU";
		private int TAM_COD_EMP = 26;
		private int TAM_CHAVE = 16;
		private string dados;
		public string numbers = "0123456789";

		public Itaucripto() {
			this.sbox = new int[256];
			this.key = new int[256];

			this.numPed = "";
			this.tipPag = "";
			this.codEmp = "";
		}

		private string Algoritmo(string dados, string chave) {
			int k = 0;
			int m = 0;

			string str = "";
			Inicializa(chave);

			for (int j = 1; j <= dados.Length; j++) {
				k = (k + 1) % 256;
				m = (m + this.sbox[k]) % 256;
				int i = this.sbox[k];
				this.sbox[k] = this.sbox[m];
				this.sbox[m] = i;

				int n = this.sbox[((this.sbox[k] + this.sbox[m]) % 256)];

				int i1 = dados[j - 1] ^ n;
				str = str + (char)i1;
			}
			return str;
		}

		private string PreencheBranco(string paramString, int paramInt) {
			string str = paramString.ToString();
			while (str.Length < paramInt) {
				str = str + " ";
			}
			return str.Substring(0, paramInt);
		}

		private string PreencheZero(string paramString, int paramInt) {
			string str = paramString.ToString();
			while (str.Length < paramInt) {
				str = "0" + str;
			}
			return str.Substring(0, paramInt);
		}

		private void Inicializa(string paramString) {
			int m = paramString.Length;
			for (int j = 0; j <= 255; j++) {
				this.key[j] = paramString[j % m];
				this.sbox[j] = j;
			}

			int k = 0;
			for (var j = 0; j <= 255; j++) {
				k = (k + this.sbox[j] + this.key[j]) % 256;
				int i = this.sbox[j];
				this.sbox[j] = this.sbox[k];
				this.sbox[k] = i;
			}
		}

		private bool isNumeric(string paramString) {
			if (paramString.Length > 1) {
				bool @bool = true;
				for (int i = 0; i < paramString.Length; i++) {
					@bool = isNumeric(paramString.Substring(i, 1));
					if (!@bool) {
						return @bool;
					}
				}
				return @bool;
			}

			if (this.numbers.IndexOf(paramString, StringComparison.Ordinal) >= 0) {
				return true;
			}
			return false;
		}

		private string Converte(string paramString) {
			char c = (char)(int)(26.0D * new Random(1).NextDouble() + 65.0D);
			string str = "" + c;

			for (int i = 0; i < paramString.Length; i++) {
				int k = paramString[i];

				int j = k;

				str = str + Convert.ToString(j);
				c = (char)(int)(26.0D * new Random(2).NextDouble() + 65.0D);
				str = str + c;
			}
			return str;
		}

		private string Desconverte(string DC) {
			string str1 = "";

			for (int i = 0; i < DC.Length; i++) {
				string str2 = "";

				char c = DC[i];

				while (char.IsDigit(c)) {
					str2 = str2 + DC[i];
					i += 1;
					c = DC[i];
				}

				if (str2.CompareTo("") != 0) {
					int j = int.Parse(str2);
					str1 = str1 + (char)j;
				}
			}
			return str1;
		}

		public virtual string geraDados(
			string codEmp,
			string pedido,
			string valor,
			string observacao,
			string chave,
			string nomeSacado,
			string codigoInscricao,
			string numeroInscricao,
			string enderecoSacado,
			string bairroSacado,
			string cepSacado,
			string cidadeSacado,
			string estadoSacado,
			string dataVencimento,
			string urlRetorna,
			string obsAdicional1,
			string obsAdicional2,
			string obsAdicional3) {
			codEmp = codEmp.ToUpper();
			chave = chave.ToUpper();

			if (codEmp.Length != this.TAM_COD_EMP) {
				return "Erro: tamanho do codigo da empresa diferente de 26 posições.";
			}
			if (chave.Length != this.TAM_CHAVE) {
				return "Erro: tamanho da chave da chave diferente de 16 posições.";
			}
			if ((pedido.Length < 1) || (pedido.Length > 8)) {
				return "Erro: número do pedido inválido.";
			}
			if (isNumeric(pedido)) {
				pedido = PreencheZero(pedido, 8);
			}
			else {
				return "Erro: numero do pedido não é numérico.";
			}
			if ((valor.Length < 1) || (valor.Length > 11)) {
				return "Erro: valor da compra inválido.";
			}

			int i = valor.IndexOf(',');
			if (i != -1) {
				string str3 = valor.Substring(i + 1);
				if (!isNumeric(str3)) {
					return "Erro: valor decimal não é numérico.";
				}
				if (str3.Length != 2) {
					return "Erro: valor decimal da compra deve possuir 2 posições após a virgula.";
				}
				valor = valor.Substring(0, valor.Length - 3) + str3;
			}
			else {
				if (!isNumeric(valor)) {
					return "Erro: valor da compra não é numérico.";
				}
				if (valor.Length > 8) {
					return "Erro: valor da compra deve possuir no máximo 8 posições antes da virgula.";
				}

				valor = valor + "00";
			}
			valor = PreencheZero(valor, 10);

			codigoInscricao = codigoInscricao.Trim();
			if ((codigoInscricao.CompareTo("02") != 0) && (codigoInscricao.CompareTo("01") != 0) && (codigoInscricao.CompareTo("") != 0)) {
				return "Erro: código de inscrição inválido.";
			}
			if ((numeroInscricao.CompareTo("") != 0) && (!isNumeric(numeroInscricao)) && (numeroInscricao.Length > 14)) {
				return "Erro: número de inscrição inválido.";
			}

			if ((cepSacado.CompareTo("") != 0) && ((!isNumeric(cepSacado)) || (cepSacado.Length != 8))) {
				return "Erro: cep inválido.";
			}

			if ((dataVencimento.CompareTo("") != 0) && ((!isNumeric(dataVencimento)) || (dataVencimento.Length != 8))) {
				return "Erro: data de vencimento inválida.";
			}
			if (obsAdicional1.Length > 60) {
				return "Erro: observação adicional 1 inválida.";
			}
			if (obsAdicional2.Length > 60) {
				return "Erro: observação adicional 2 inválida.";
			}
			if (obsAdicional3.Length > 60) {
				return "Erro: observação adicional 3 inválida.";
			}

			observacao = PreencheBranco(observacao, 40);
			nomeSacado = PreencheBranco(nomeSacado, 30);
			codigoInscricao = PreencheBranco(codigoInscricao, 2);
			numeroInscricao = PreencheBranco(numeroInscricao, 14);
			enderecoSacado = PreencheBranco(enderecoSacado, 40);
			bairroSacado = PreencheBranco(bairroSacado, 15);
			cepSacado = PreencheBranco(cepSacado, 8);
			cidadeSacado = PreencheBranco(cidadeSacado, 15);
			estadoSacado = PreencheBranco(estadoSacado, 2);
			dataVencimento = PreencheBranco(dataVencimento, 8);
			urlRetorna = PreencheBranco(urlRetorna, 60);
			obsAdicional1 = PreencheBranco(obsAdicional1, 60);
			obsAdicional2 = PreencheBranco(obsAdicional2, 60);
			obsAdicional3 = PreencheBranco(obsAdicional3, 60);

			string str1 = Algoritmo(pedido + valor + observacao + nomeSacado + codigoInscricao + numeroInscricao + enderecoSacado + bairroSacado + cepSacado + cidadeSacado + estadoSacado + dataVencimento + urlRetorna + obsAdicional1 + obsAdicional2 + obsAdicional3, chave);

			string str2 = Algoritmo(codEmp + str1, this.CHAVE_ITAU);

			return Converte(str2);
		}

		public virtual string geraCripto(
			string codEmp,
			string codSacado,
			string chave) {
			if (codEmp.Length != this.TAM_COD_EMP) {
				return "Erro: tamanho do codigo da empresa diferente de 26 posições.";
			}
			if (chave.Length != this.TAM_CHAVE) {
				return "Erro: tamanho da chave da chave diferente de 16 posições.";
			}
			codSacado = codSacado.Trim();
			if (codSacado.CompareTo("") == 0) {
				return "Erro: código do sacado inválido.";
			}

			string str1 = Algoritmo(codSacado, chave);

			string str2 = Algoritmo(codEmp + str1, this.CHAVE_ITAU);

			return Converte(str2);
		}

		public virtual string geraConsulta(
			string codEmp,
			string pedido,
			string formato,
			string chave) {
			if (codEmp.Length != this.TAM_COD_EMP) {
				return "Erro: tamanho do codigo da empresa diferente de 26 posições.";
			}
			if (chave.Length != this.TAM_CHAVE) {
				return "Erro: tamanho da chave da chave diferente de 16 posições.";
			}
			if ((pedido.Length < 1) || (pedido.Length > 8)) {
				return "Erro: número do pedido inválido.";
			}
			if (isNumeric(pedido)) {
				pedido = PreencheZero(pedido, 8);
			}
			else {
				return "Erro: numero do pedido não é numérico.";
			}
			if ((formato.CompareTo("0") != 0) && (formato.CompareTo("1") != 0)) {
				return "Erro: formato inválido.";
			}

			string str1 = Algoritmo(pedido + formato, chave);

			string str2 = Algoritmo(codEmp + str1, this.CHAVE_ITAU);

			return Converte(str2);
		}

		public virtual string decripto(
			string DC,
			string chave) {
			DC = Desconverte(DC);
			string str = Algoritmo(DC, chave);

			this.codEmp = str.Substring(0, 26);

			this.numPed = str.Substring(26, 8);

			this.tipPag = str.Substring(34, 2);
			return str;
		}

		public virtual string retornaCodEmp() {
			return this.codEmp;
		}

		public virtual string retornaPedido() {
			return this.numPed;
		}

		public virtual string retornaTipPag() {
			return this.tipPag;
		}

		public virtual string geraDadosGenerico(
			string codEmp,
			string dados,
			string chave) {
			codEmp = codEmp.ToUpper();
			chave = chave.ToUpper();

			if (codEmp.Length != this.TAM_COD_EMP) {
				return "Erro: tamanho do codigo da empresa diferente de 26 posições.";
			}
			if (chave.Length != this.TAM_CHAVE) {
				return "Erro: tamanho da chave da chave diferente de 16 posições.";
			}
			if (dados.Length < 1) {
				return "Erro: sem dados.";
			}

			string str1 = Algoritmo(dados, chave);

			string str2 = Algoritmo(codEmp + str1, this.CHAVE_ITAU);

			return Converte(str2);
		}
	}
}
