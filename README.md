# plugitstudio

PlugIt Studio é uma aplicação genérica construída para compilar e rodar plugins.

## Comandos

 - Alt + ": Adicionar Plugin
 - Ctrl + ": Encerrar aplicação

## Plugins

Plugins são .zip de arquivos do projeto com extensão renomeada para .plugin. PlugItStudio é baseado em componentes. Um componente é um arquivo .pic (Plug It Component), um arquivo de texto comum. Desta forma, criar plugins é fácil.

## Componentes

Componentes são código C# com um pouco de pré-processamento. As funções que descrevem o funcionamento desses componentes são definidas com a palavra-chave **behavior** da seguinte forma:

```
behavior draw
{
	clear(color((byte)(r % 255), 0, 0));
}
```

Existem vários comportamentos que podem ser implementados:

 - draw: Código de desenho do componente
 - start: Código executado quando o componente é instânciado.
 - load: Código executado quando o componente tem seu protótipo carregado.
 - key: Código executado quando o componente está presente e escuta um caractér.
 - up: Código executado quando o componente está presente e escuta um evento MouseUp.
 - down: Código executado quando o componente está presente e escuta um evento MouseDown.
 - move: Código executado quando o componente está presente e escuta um evento MouseMove.

Para criar um componente basta utilizar o comando create [nome do arquivo do componente].

## Implementações Futuras

- [ ] Adição de plugins dentro de ourtos plugins
- [ ] Aplicar separation of concerns no formulário principal
- [ ] Adicionar todas as funções da classe Graphics como alias.
- [ ] Criar bilbioteca compartilhada pela aplicação principal e pelos plugins.
- [ ] Melhorar processo de criação dos componentes, trazendo mais possibilidades.
- [ ] Adicionar behaviors válidos.
- [ ] Adicionar plugins de compilação.