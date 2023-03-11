using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        ICommentRepository _commentRepository;

        public CommentManager(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public void TAdd(Comment t)
        {
            _commentRepository.Insert(t);
        }

        public void TDelete(Comment t)
        {
            _commentRepository.Delete(t);
        }

        public Comment TGetByID(int id)
        {
            return _commentRepository.GetByID(id);
        }

        public List<Comment> TGetList()
        {
            return _commentRepository.GetList();
        }

        public void TUpdate(Comment t)
        {_commentRepository.Update(t);
        }
    }
}
