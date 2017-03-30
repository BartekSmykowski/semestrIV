package lab2pt.model;


import javafx.application.Platform;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.beans.property.SimpleStringProperty;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;

public class ImageProcessingJob {
    private File file;
    private SimpleStringProperty status;
    private DoubleProperty progres;

    public ImageProcessingJob(File file){
        this.file = file;
        status = new SimpleStringProperty("Waiting.");
        progres = new SimpleDoubleProperty(0);

    }

    public void convertToGrayscale(File originalFile, //oryginalny plik graficzny
                                    File outputDir //katalog docelowy
    ) {
        try {     //wczytanie oryginalnego pliku do pamięci
            BufferedImage original = ImageIO.read(originalFile);          //przygotowanie bufora na grafikę w skali szarości
            BufferedImage grayscale = new BufferedImage(original.getWidth(), original.getHeight(), original.getType());
            status.set("Processing...");
            //przetwarzanie piksel po pikselu
            for (int i = 0; i < original.getWidth(); i++) {
                for (int j = 0; j < original.getHeight(); j++) {

                    //pobranie składowych RGB
                    int red = new Color(original.getRGB(i, j)).getRed();
                    int green = new Color(original.getRGB(i, j)).getGreen();
                    int blue = new Color(original.getRGB(i, j)).getBlue();

                    //obliczenie jasności piksela dla obrazu w skali szarości
                    int luminosity = (int) (0.21 * red + 0.71 * green + 0.07 * blue);             //przygotowanie wartości koloru w oparciu o obliczoną jaskość
                    int newPixel = new Color(luminosity, luminosity, luminosity).getRGB();

                    //zapisanie nowego piksela w buforze
                    grayscale.setRGB(i, j, newPixel);
                }

                //obliczenie postępu przetwarzania jako liczby z przedziału [0, 1]
                double progress = (1.0 + i) / original.getWidth();         //aktualizacja własności zbindowanej z paskiem postępu w tabeli
                Platform.runLater(() -> progres.set(progress));
            }
            status.set("Completed.");
            //przygotowanie ścieżki wskazującej na plik wynikowy
            Path outputPath = Paths.get(outputDir.getAbsolutePath(), originalFile.getName());          //zapisanie zawartości bufora do pliku na dysku
            ImageIO.write(grayscale, "jpg", outputPath.toFile());

        } catch (
                IOException ex)

        {     //translacja wyjątku
            throw new RuntimeException(ex);
        }
    }

    public double getProgres() {
        return progres.get();
    }

    public DoubleProperty progresProperty() {
        return progres;
    }

    public void setProgres(double progres) {
        this.progres.set(progres);
    }

    public String getStatus() {
        return status.get();
    }

    public SimpleStringProperty statusProperty() {
        return status;
    }

    public void setStatus(String status) {
        this.status.set(status);
    }

    public File getFile() {
        return file;
    }

    public void setFile(File file) {
        this.file = file;
    }
}
