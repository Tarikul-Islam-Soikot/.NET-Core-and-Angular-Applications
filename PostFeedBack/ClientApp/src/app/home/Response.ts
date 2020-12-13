export class PostDto {
  constructor() {
    this.Comments = [];
  }

  postID: number;
  PostName: string;
  PostCreatorName: string;
  PostCreationDate: Date;
  NumOfComments: number;
  Comments: CommentsDto[];
}

export class CommentsDto {
  PostID: number;
  CommentID: number;
  CommentDescription: string;
  CommentCreatorName: string;
  CommentCreationDate: Date;
  NumOfLike: number;
  NumOfDislike: number;
}
