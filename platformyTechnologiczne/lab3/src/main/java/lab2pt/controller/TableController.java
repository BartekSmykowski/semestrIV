package lab2pt.controller;

import javafx.application.Platform;
import javafx.beans.property.SimpleStringProperty;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.control.cell.ProgressBarTableCell;
import lab2pt.model.ImageProcessingJob;
import lab2pt.model.ImageProcessingJobsContainer;

import java.io.File;
import java.net.URL;
import java.util.ResourceBundle;
import java.util.concurrent.ForkJoinPool;

public class TableController {
    public TableColumn<ImageProcessingJob, String> imageNameColumn;
    public TableColumn<ImageProcessingJob, Double> progressColumn;
    public TableColumn<ImageProcessingJob, String> statusColumn;
    public TableView<ImageProcessingJob> processingTable;
    public Label processingTimeLabel;
    public Label parallelProcessLabel;
    public ChoiceBox numberOfCoresChoiceBox;
    public Button normalProcessingButton;
    public Button parallelProcessingButton;

    private ImageProcessingJobsContainer imageProcessingJobsContainer;

    @FXML
    public void initialize(URL url, ResourceBundle rb) {

    }


    public void setImageProcessingJobsContainer(ImageProcessingJobsContainer imageProcessingJobsContainer) {
        this.imageProcessingJobsContainer = imageProcessingJobsContainer;
        processingTable.setItems(imageProcessingJobsContainer.getJobs());

        imageNameColumn.setCellValueFactory(p -> new SimpleStringProperty(p.getValue().getFile().getName()));

        statusColumn.setCellValueFactory(p -> p.getValue().statusProperty());

        progressColumn.setCellFactory(ProgressBarTableCell.<ImageProcessingJob>forTableColumn());

        progressColumn.setCellValueFactory(p -> p.getValue().progresProperty().asObject());

    }

    private void normalBackgroundJob() {
        long start = System.currentTimeMillis();
        imageProcessingJobsContainer.getJobs().forEach(p->p.convertToGrayscale(p.getFile(), new File(p.getFile().getParent() + "/converted")));
        long end = System.currentTimeMillis();
        long duration = end-start;
        Platform.runLater(()
                -> processingTimeLabel.setText("Czas konwersji: "
                + Long.toString(end - start) + " ms"));
    }

    private void parallelBackgroundJob() {
        long start = System.currentTimeMillis();
        imageProcessingJobsContainer.getJobs().parallelStream().forEach(p->p.convertToGrayscale(p.getFile(), new File(p.getFile().getParent() + "/converted")));
        long end = System.currentTimeMillis();
        long duration = end-start;
        Platform.runLater(()
                -> parallelProcessLabel.setText("Czas konwersji: "
                + Long.toString(end - start) + " ms"));
    }

    public void parallelProcessing(ActionEvent actionEvent) {
        ForkJoinPool pool = new ForkJoinPool(Integer.parseInt(numberOfCoresChoiceBox.getSelectionModel().getSelectedItem().toString()));
        pool.submit(this::parallelBackgroundJob);
        pool.shutdown();
    }

    public void normalProcessing(ActionEvent actionEvent) {
        new Thread(this::normalBackgroundJob).start();
    }
}
