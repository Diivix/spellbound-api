FROM gliderlabs/alpine:latest
RUN apk add --update sqlite
RUN mkdir /data
RUN /usr/bin/sqlite3 /data/spellbound.db
CMD /bin/sh

# docker run -it -v /home/dbfolder/:/db imagename