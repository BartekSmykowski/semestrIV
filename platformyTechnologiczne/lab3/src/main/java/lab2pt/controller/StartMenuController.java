package lab2pt.controller;

import javafx.application.Platform;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import lab2pt.model.ImageProcessingJob;
import lab2pt.model.ImageProcessingJobsContainer;

import java.io.File;
import java.util.List;

public class StartMenuController {

    @FXML
    public Button exit;
    @FXML
    public Button fileChoose;
    @FXML
    public Button processButton;
    private List<File> selectedFiles;
    private ObservableList<ImageProcessingJob> jobs;

    public void exitApplication() throws Exception {
        Platform.exit();
    }

    public void openFileChooser(ActionEvent actionEvent) {
        FileChooser fileChooser = new FileChooser();
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("JPG images", "*.jpg"));
        fileChooser.setTitle("Choose images.");
        selectedFiles = fileChooser.showOpenMultipleDialog(null);

    }

    @FXML
    void processFiles(ActionEvent event) throws Exception {


        ImageProcessingJobsContainer imageProcessingJobsContainer = new ImageProcessingJobsContainer();
        for (File element : selectedFiles) {
            imageProcessingJobsContainer.addJob(new ImageProcessingJob(element));
        }
        jobs = imageProcessingJobsContainer.getJobs();

        FXMLLoader loader = new FXMLLoader(getClass().getResource("/tableScene.fxml"));
        Parent root = loader.load();
        root.getStylesheets().clear();
        root.getStylesheets().add(getClass().getResource("/Dark.css").toString());
        TableController tableController = loader.getController();
        tableController.setImageProcessingJobsContainer(imageProcessingJobsContainer);

        Stage stage = (Stage) processButton.getScene().getWindow();
        stage.setScene(new Scene(root, 600, 600));
        stage.show();


    }


}
