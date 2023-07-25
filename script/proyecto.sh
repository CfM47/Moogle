#!/usr/bin/env bash
option=$1;
editor=$2;

case $option in
    "run")
        dotnet watch run --project ../MoogleServer
        ;;
    "report")
        pdflatex -synctex=1 -interaction=nonstopmode ../informe/Informe\ moogle\ Latex.tex
        ;;
    "show_report")
        okular ../Informe/Informe\ moogle\ Latex.pdf
        ;;
    "slides")
        pdflatex -synctex=1 -interaction=nonstopmode ../presentacion/main.tex
        ;;
    "show_slides")
        okular ../presentacion/main.pdf
        ;;
    "clean")
        rm ../presentacion/main.log
        rm ../presentacion/main.pdf
        rm ../presentacion/main.synctex.gz
        rm ../presentacion/main.vrb
        rm ../presentacion/main.toc
        rm ../presentacion/main.snm
        rm ../presentacion/main.nav
        rm ../presentacion/main.aux
        rm ../presentacion/sections/clases.aux
        rm ../presentacion/sections/estructura.aux
        rm ../presentacion/sections/funcionamiento.aux
        rm ../presentacion/sections/introduccion.aux
        rm ../presentacion/sections/otras.aux
        rm ../informe/Informe\ moogle\ Latex.aux
        rm ../informe/Informe\ moogle\ Latex.log
        rm ../informe/Informe\ moogle\ Latex.pdf
        rm ../informe/Informe\ moogle\ Latex.synctex.gz

        
        ;;
esac


echo "Hasdf"