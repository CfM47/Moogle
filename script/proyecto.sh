#!/usr/bin/bash
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
        file="../Informe/Informe\ moogle\ Latex.pdf";
        if [[! -f $file ]] 
        then
            pdflatex -synctex=1 -interaction=nonstopmode ../informe/Informe\ moogle\ Latex.tex
        fi
        if [[ "$editor" -eq "" ]]
        then
            okular $file
        else
            command='$editor $file'
            eval " $command"
        fi        
        ;;
    "slides")
        pdflatex -synctex=1 -interaction=nonstopmode ../presentacion/main.tex
        ;;
    "show_slides")
        file="../presentacion/main.pdf";
        if [[! -f $file ]] 
        then
            pdflatex -synctex=1 -interaction=nonstopmode ../presentacion/main.tex
        fi
        if [[ "$editor" -eq "" ]]
        then
            okular $file
        else
            command='$editor $file'
            eval " $command"
        fi    
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
        rm ../DatabaseInfo.json
        rm -r ../MoogleServer/bin/
        rm -r ../MoogleServer/obj/
        rm -r ../MoogleEngine/bin/
        rm -r ../MoogleEngine/obj/
        rm -r ../bin/
        rm -r ../obj/           
        ;;
esac


echo "done"
