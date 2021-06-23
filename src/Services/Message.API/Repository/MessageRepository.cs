﻿using AutoMapper;
using Message.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Message.API.Models;

namespace Message.API.Repository
{
    public class MessageRepository:IMessageRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Message> _db;
        private readonly IMapper _mapper;

        public MessageRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _db = _context.Set<Message>();
            _mapper = mapper;
        }

        public async Task<IList<MessageDto>> GetAllByRoomId(int roomId)
        {
            IQueryable<Message> query = _db;

            query = query.Where(m => m.ToRoomId == roomId);
            var messages = await query.AsNoTracking().ToListAsync();

            return _mapper.Map<IList<MessageDto>>(messages);
        }

        public async Task<MessageDto> Get(int id)
        {
            IQueryable<Message> query = _db;
            var result = await query.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<MessageDto>(result);
        }

        public async Task<MessageDto> SaveMessage(MessageDto message)
        {
            message.CreatedAt = new DateTime();
            var result = _mapper.Map<Message>(message);
            await _db.AddAsync(result);
            return _mapper.Map<MessageDto>(result);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _db.Remove(entity);
        }

        public async void Update(MessageDto messageDto)
        {
            var result = _mapper.Map<Message>(messageDto);
            _db.Attach(result);
            _context.Entry(result).State = EntityState.Modified;
        }
    }
}