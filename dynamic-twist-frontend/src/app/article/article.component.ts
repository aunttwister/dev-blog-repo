import { Component, EventEmitter, Input, OnInit } from '@angular/core'
import { ArticleService } from '../services/article.service'
import { Article } from '../models/article';
import { ActivatedRoute, PRIMARY_OUTLET, Router } from '@angular/router';

@Component({
  selector: 'app-blog',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.css'],
  providers: [ArticleService]
})

export class ArticleComponent implements OnInit {
  @Input() update!: EventEmitter<string>;

  articles = new Array<Article>();
  latestArticle!: Article;
  isLoaded = false;
  sourceType = '';

  constructor(private articleService: ArticleService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params =>
      {
        this.sourceType = params.get("type") as string
        this.getFiles(this.sourceType);
      });
  }

  getFiles(sourceType: string){
    return this.articleService.getArticles(sourceType).subscribe(data => {
      this.formatMarkdown(data)
    });
  }

  formatMarkdown(data: Article[]){
      this.latestArticle = data[0];
      data.forEach(article => {
        article.fullName = encodeURIComponent(article.fullName)
      });
      this.articles = data;
      this.isLoaded = true;
  }

  reloadCurrentRoute() {
    const currentUrl = this.router.url;
    this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
        this.router.navigate([currentUrl]);
    });
  }
}
