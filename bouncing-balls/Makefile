#
# Copyright (c) 2021 jdeokkim
#

.PHONY: all clean

PROJECT_NAME := c-krit/ferox
PROJECT_PATH := ../../ferox/ferox
PROJECT_PREFIX := $(shell tput setaf 2)$(PROJECT_NAME):$(shell tput sgr0)

RAYLIB_PATH ?= ../../raylib

BINARY_PATH := bin
INCLUDE_PATH := $(PROJECT_PATH)/include
LIBRARY_PATH := $(PROJECT_PATH)/lib
SOURCE_PATH := src

EXAMPLES := bouncing_balls
    
SOURCES := $(EXAMPLES:%=$(SOURCE_PATH)/%.c)
TARGETS := $(EXAMPLES:%=$(BINARY_PATH)/%)

HOST_OS := LINUX
TARGET_OS := $(HOST_OS)

ifeq ($(OS),Windows_NT)
	PROJECT_PREFIX := $(PROJECT_NAME):
	HOST_OS := WINDOWS
else
	UNAME = $(shell uname)
	ifeq ($(UNAME),Linux)
		HOST_OS = LINUX
	endif
endif

CC := gcc
CFLAGS := -D_DEFAULT_SOURCE -DHAVE_RAYLIB -g $(INCLUDE_PATH:%=-I%) -std=gnu99 -O2
CFLAGS += -I$(RAYLIB_PATH)/src 
LDFLAGS := -L$(LIBRARY_PATH)
LDFLAGS += -L$(RAYLIB_PATH)/src
LDLIBS := -lferox -lraylib -lGL -lm -lpthread -ldl -lrt -lX11

ifeq ($(TARGET_OS),WINDOWS)
	TARGETS := $(EXAMPLES:%=$(BINARY_PATH)/%.exe)
	CC := x86_64-w64-mingw32-gcc
	LDLIBS := -lferox -lraylib -lopengl32 -lgdi32 -lwinmm -lpthread
endif

all: pre-build build post-build

pre-build:
	@echo "$(PROJECT_PREFIX) Using: '$(CC)' to build all examples."
    
build: $(TARGETS)

$(BINARY_PATH)/%: $(SOURCE_PATH)/%.c
	@mkdir -p $(BINARY_PATH)
	@echo "$(PROJECT_PREFIX) Compiling: $@ (from $<)"
	@$(CC) $< -o $@ $(CFLAGS) $(LDFLAGS) $(LDLIBS)
    
$(BINARY_PATH)/%.exe: $(SOURCE_PATH)/%.c
	@mkdir -p $(BINARY_PATH)
	@echo "$(PROJECT_PREFIX) Compiling: $@ (from $<)"
	@$(CC) $< -o $@ $(CFLAGS) $(LDFLAGS) $(LDLIBS)
    
post-build:
	@echo "$(PROJECT_PREFIX) Build complete."

clean:
	@echo "$(PROJECT_PREFIX) Cleaning up."
	@rm -rf $(BINARY_PATH)/*