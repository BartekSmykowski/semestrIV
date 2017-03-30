package lab2pt.model;

import javafx.collections.FXCollections;
import javafx.collections.ObservableList;

public class ImageProcessingJobsContainer {

    private ObservableList<ImageProcessingJob> jobs;

    public ImageProcessingJobsContainer(){
        this.jobs  = FXCollections.observableArrayList();
    }

    public void addJob(ImageProcessingJob imageProcessingJob){
        jobs.add(imageProcessingJob);
    }

    public ObservableList<ImageProcessingJob> getJobs() {
        return jobs;
    }

    public void setJobs(ObservableList<ImageProcessingJob> jobs) {
        this.jobs = jobs;
    }
}
