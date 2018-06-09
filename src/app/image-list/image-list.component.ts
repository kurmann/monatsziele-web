import { Component, OnInit } from '@angular/core';
import { ImageService } from '../image-service/image.service';
import { UnsplashImage } from '../image-service/UnsplashImage';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-image-list',
  templateUrl: './image-list.component.html',
  styleUrls: ['./image-list.component.css']
})
export class ImageListComponent implements OnInit {

  images: UnsplashImage[];
  constructor(private _imageService: ImageService) { }

  ngOnInit() {
    this._imageService.getImages().subscribe(images => {
      this.images = images;
    });
  }

}